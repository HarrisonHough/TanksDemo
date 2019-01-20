using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class NetworkGameManager : NetworkBehaviour
{
    #region Singleton 
    private static NetworkGameManager instance;
    [SerializeField]
    private bool dontDestroyOnLoad = false;

    //publicly accessible reference to the instance
    public static NetworkGameManager Instance
    {
        get
        {
            //if instance exists return instance
            return instance;
        }
    }

    /// <summary>
    /// Called on awake, before the start function
    /// </summary>
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as NetworkGameManager;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    public static List<NetworkPlayerController> AllPlayers = new List<NetworkPlayerController>();

    public NetworkLobbyManager networkLobbyManager;
    [SerializeField]
    private UIControl uiControl;

    [SyncVar]
    public GameState Gamestate = GameState.InGame;

    private bool gameOver = false;

    public int MaxScore = 3;

    NetworkPlayerController winningPlayer;
    [SerializeField]
    NetworkFXController fxController;

    public NetworkPool BulletPool;
    [SerializeField]
    private SpawnPoint[] spawnPoints;
    [SerializeField]
    private LeaderboardUI leaderboardUI;

    [Server]
    void Start()
    {
        StartCoroutine(GameLoopRoutine());
    }

    public void SpawnHitFX(Vector3 position)
    {
        //RpcSpawnHitFX(position);
        fxController.SpawnHitFX(position);
    }
    [ClientRpc]
    private void RpcSpawnHitFX(Vector3 position)
    {
        Debug.Log("Called Rpc Spawn HIt");
        fxController.SpawnHitFX(position);
    }
    public void SpawnFX(Vector3 position)
    {
        fxController.SpawnFX(position);
    }

    [ClientRpc]
    public void RpcSpawnFX(Vector3 position)
    {
        fxController.SpawnFX(position);
    }

    public void SpawnExplodeFX(Vector3 position)
    {
        fxController.SpawnExplodeFX(position);
    }


    public void GameOver()
    {

        //show gameOver UI
        uiControl.ToggleGameOverPanel(true);
        Gamestate = GameState.GameOver;

    }

    public void Restart()
    {

    }

    public void BackToHomeScene()
    {
        SceneManager.LoadScene(0);
    }

    public Transform GetSpawnPoint()
    {

        if (spawnPoints != null)
        {
            if (spawnPoints.Length > 0)
            {
                bool foundSpawnPoint = false;
                Transform newSpawnPoint = transform;
                float timeOut = Time.time + 2f;

                while (!foundSpawnPoint)
                {
                    newSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
                    if (!newSpawnPoint.GetComponent<SpawnPoint>().CheckIsOccupied())
                    {
                        foundSpawnPoint = true;

                    }
                    if (Time.time > timeOut)
                    {
                        foundSpawnPoint = true;
                        newSpawnPoint = transform;
                    }
                }
                return newSpawnPoint;
            }
        }
        return transform;
    }

    [Server]
    public void UpdateScoreboard()
    {
        string[] playerNames = new string[AllPlayers.Count];
        int[] playerScores = new int[AllPlayers.Count];

        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers != null)
            {
                playerNames[i] = AllPlayers[i].GetComponent<NetworkPlayerSetup>().PlayerName;
                playerScores[i] = AllPlayers[i].Score;
            }

        }
        RpcUpdateScoreboard(playerNames, playerScores);
    }

    [ClientRpc]
    void RpcUpdateScoreboard(string[] playerNames, int[] playerScores)
    {

        leaderboardUI.UpdateScores(playerNames, playerScores);
    }

    IEnumerator GameLoopRoutine()
    {
        LobbyManager lobby = LobbyManager.s_Singleton;

        while (AllPlayers.Count < lobby._playerNumber)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(EndGameRoutine());

        Gamestate = GameState.InGame;
        yield return null;

    }

    [ClientRpc]
    void RpcStartGame()
    {
        //UpdateMessage("FIGHT");

        DisablePlayerControls();
    }

    IEnumerator StartGameRoutine()
    {
        Reset();
        RpcStartGame();
        UpdateScoreboard();
        yield return null;
    }

   

    [ClientRpc]
    void RpcPlayGame()
    {
        EnablePlayerControls();

        //UpdateMessage("");
    }

    IEnumerator PlayGameRoutine()
    {
        yield return new WaitForSeconds(1f);
        Gamestate = GameState.InGame;
        RpcPlayGame();

        while (gameOver == false)
        {
            CheckScores();
            yield return null;
        }
    }

    IEnumerator EndGameRoutine()
    {
        RpcEndGame();
        //update winner message
        yield return new WaitForSeconds(3f);

        Reset();
        LobbyManager.s_Singleton._playerNumber = 0;
        LobbyManager.s_Singleton.SendReturnToLobby();
    }

    [ClientRpc]
    void RpcEndGame()
    {
        DisablePlayerControls();

    }

    IEnumerator EndGame()
    {
        RpcEndGame();
        //RpcUpdateMessage("GAME OVER \n " + Winner.pSetup.name + " wins!");
        yield return new WaitForSeconds(3f);
        Reset();

        LobbyManager.s_Singleton._playerNumber = 0;
        LobbyManager.s_Singleton.SendReturnToLobby();

    }

    public void CheckScores()
    {
        winningPlayer = GetWinner();

        if (winningPlayer != null)
        {
            gameOver = true;
        }
    }

    NetworkPlayerController GetWinner()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i].Score >= MaxScore)
            {
                return AllPlayers[i];
            }
        }

        return null;
    }

    private void EnablePlayerControls()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i] != null)
            {
                AllPlayers[i].EnablePlayerControls();
            }
        }
    }

    private void DisablePlayerControls()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i] != null)
            {
                AllPlayers[i].DisablePlayerControls();
            }
        }
    }

    void Reset()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            AllPlayers[i].Reset();
        }
    }


}
