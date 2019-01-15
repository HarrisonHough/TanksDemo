using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class NetworkGameManager : NetworkBehaviour
{
    #region Singleton 
    static NetworkGameManager instance;

    public static NetworkGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NetworkGameManager>();

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<NetworkGameManager>();
                }
            }
            return instance;
        }
    }
    #endregion


    [SerializeField]
    private Pool hitFXPool;
    public Pool HitFXPool { get { return hitFXPool; } }
    [SerializeField]
    private Pool spawnFXPool;
    public Pool SpawnFXPool { get { return SpawnFXPool; } }
    [SerializeField]
    private Pool explodeFXPool;
    public Pool ExplodeFXPool { get { return explodeFXPool; } }
    [SerializeField]
    private Pool bulletPool;
    public Pool BulletPool { get { return bulletPool; } }


    public static List<NetworkPlayerController> AllPlayers = new List<NetworkPlayerController>(); 

    public NetworkLobbyManager networkLobbyManager;
    [SerializeField]
    private UIControl uiControl;

    [SyncVar]
    public GameState Gamestate;

    private bool gameOver = false;

    public int MaxScore = 3;

    NetworkPlayerController winningPlayer;

    [Server]
    void Start()
    {
        StartCoroutine(GameLoopRoutine());
    }



    public void GameOver()
    {
        //update ui panel score
        //uiControl.UpdateScoreText(tanksDestroyed);

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

    void EnablePlayers()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i] != null)
            {
                AllPlayers[i].Enable();
            }
        }
    }

    void DisablePlayers()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i] != null)
            {
                AllPlayers[i].Disable();
            }
        }
    }

    IEnumerator GameLoopRoutine()
    {
        LobbyManager lobbyManager = LobbyManager.s_Singleton;

        if (lobbyManager != null)
        {

            while (AllPlayers.Count < lobbyManager._playerNumber)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(StartGame());
            yield return StartCoroutine(PlayGame());
            yield return StartCoroutine(EndGame());
            StartCoroutine(GameLoopRoutine());
        }

        

        yield return null;
    }


    [ClientRpc]
    void RpcStartGame()
    {
        DisablePlayers();
    }

    IEnumerator StartGame()
    {
        //Reset();
        RpcStartGame();
        //UpdateScoreboard();
        yield return new WaitForSeconds(3f);

    }

    [ClientRpc]
    void RpcPlayGame()
    {
        EnablePlayers();
        Gamestate = GameState.InGame;
        //UpdateMessage("");
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(1f);

        RpcPlayGame();

        while (gameOver == false)
        {
            CheckScores();
            yield return null;
        }

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
            if (AllPlayers[i].score >= MaxScore)
            {
                return AllPlayers[i];
            }
        }

        return null;
    }

    [ClientRpc]
    void RpcEndGame()
    {
        DisablePlayers();

    }

    IEnumerator EndGame()
    {
        RpcEndGame();
        //RpcUpdateMessage("GAME OVER \n " + m_winner.m_pSetup.m_name + " wins!");
        yield return new WaitForSeconds(3f);
        //Reset();

        LobbyManager.s_Singleton._playerNumber = 0;
        LobbyManager.s_Singleton.SendReturnToLobby();

    }
}
