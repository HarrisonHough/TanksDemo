using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    public NetworkLobbyManager networkLobbyManager;
    [SerializeField]
    private UIControl uiControl;

    public GameState Gamestate;

    // Start is called before the first frame update
    void Start()
    {
        Gamestate = GameState.InGame;
        //networkLobbyManager.
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
