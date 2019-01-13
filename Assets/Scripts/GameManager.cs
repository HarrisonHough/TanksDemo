using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {  InMenu, InGame, GameOver }
public class GameManager : GenericSingleton<GameManager>
{

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
    [SerializeField]
    private GameObject player;
    public GameObject Player { get { return player; } }
    [SerializeField]
    private EnemySpawner spawner;

    private TankController playerTank;

    [SerializeField]
    private UIControl uiControl;

    private int enemyCount;
    public int EnemyCount
    {
        get { return enemyCount; }

        set {
            enemyCount = value;

            if (enemyCount < 0)
                enemyCount = 0;
        }
    }

    private List<AITankController> activeAITanks = new List<AITankController>();
    public List<AITankController> ActiveAITanks { get { return activeAITanks; } }

    public GameState Gamestate;

    private int tanksDestroyed = 0;
    public int TanksDestroyed
    {
        get { return tanksDestroyed; }
        set { tanksDestroyed = value;
            uiControl.UpdateScoreText(GameManager.Instance.tanksDestroyed);
        } }

    // Start is called before the first frame update
    void Start()
    {
        Gamestate = GameState.InGame;
        
    }



    public void GameOver()
    {
        //update ui panel score
        uiControl.UpdateScoreText(tanksDestroyed);

        spawner.StopSpawning();
        //show gameOver UI
        uiControl.ToggleGameOverPanel(true);
        Gamestate = GameState.GameOver;

    }

    public void Restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        for (int i = 0; i < activeAITanks.Count; i++)
        {
            activeAITanks[i].gameObject.SetActive(false);
        }
        //reset
        tanksDestroyed = 0;

        player.transform.position = Vector3.zero;
        player.SetActive(true);
        Gamestate = GameState.InGame;
        //hide game over panel
        //uiControl.UpdateScoreText(tanksDestroyed);
        uiControl.ToggleInGameUI(true);
        spawner.RestartSpawnLoop();
    }

    public void BackToHomeScene()
    {
        SceneManager.LoadScene(0);
    }
}
