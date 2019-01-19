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

    [Server]
    void Start()
    {
        //StartCoroutine(GameLoopRoutine());
        Gamestate = GameState.InGame;
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

}
