using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {  InMenu, InGame, GameOver }
public class GameManager : GenericSingleton<GameManager>
{

    [SerializeField]
    private Pool _hitFXPool;
    public Pool HitFXPool { get { return _hitFXPool; } }
    [SerializeField]
    private Pool _spawnFXPool;
    public Pool SpawnFXPool { get { return _spawnFXPool; } }
    [SerializeField]
    private Pool _explodeFXPool;
    public Pool ExplodeFXPool { get { return _explodeFXPool; } }
    [SerializeField]
    private Pool _bulletPool;
    public Pool BulletPool { get { return _bulletPool; } }
    [SerializeField]
    private GameObject _player;
    public GameObject Player { get { return _player; } }
    [SerializeField]
    private EnemySpawner _spawner;

    private TankController _playerTank;

    [SerializeField]
    private UIControl _uiControl;

    private int _enemyCount;
    public int EnemyCount
    {
        get { return _enemyCount; }

        set {
            _enemyCount = value;

            if (_enemyCount < 0)
                _enemyCount = 0;
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
            _uiControl.UpdateScoreText(GameManager.Instance.tanksDestroyed);
        } }

    // Start is called before the first frame update
    void Start()
    {
        Gamestate = GameState.InGame;
        
    }



    public void GameOver()
    {
        //update ui panel score
        _uiControl.UpdateScoreText(tanksDestroyed);

        _spawner.StopSpawning();
        //show gameOver UI
        _uiControl.ToggleGameOverPanel(true);
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

        _player.transform.position = Vector3.zero;
        _player.SetActive(true);
        Gamestate = GameState.InGame;
        //hide game over panel
        //uiControl.UpdateScoreText(tanksDestroyed);
        _uiControl.ToggleInGameUI(true);
        _spawner.RestartSpawnLoop();
    }

    public void BackToHomeScene()
    {
        SceneManager.LoadScene(0);
    }
}
