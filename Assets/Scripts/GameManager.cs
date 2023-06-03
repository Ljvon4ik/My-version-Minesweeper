using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private LevelsDatabase _levelsDatabase;
    private int _currentLevelNumber;
    private StorageUIReference _storageUIReference;
    private Button _startButton;

    ITimer _timer;
    IMediator _mediator;
    StartMenuCreator _startMenuCreator;

    [Inject]
    private void Construct(LevelsDatabase levelsDatabase, StorageUIReference storageUIReference, IMediator mediator)
    {
        _levelsDatabase = levelsDatabase;
        _storageUIReference = storageUIReference;
        _mediator = mediator;
        _startMenuCreator = storageUIReference.StartMenu.GetComponent<StartMenuCreator>();
        _startButton = storageUIReference.StartButton;
    }

    private void Awake()
    {
        EventManager.EndGameBool.AddListener(EndGame);
        EventManager.OnFirstClick.AddListener(delegate { _timer.StartTimer(); });
        EventManager.EndGame.AddListener(delegate { _timer.StopTimer(); });       
    }
    private void Start()
    {
        _timer = new Timer();
        _startButton.onClick.AddListener(CreatLevel);
        _storageUIReference.RestartButton.onClick.AddListener(Restart);
        _storageUIReference.ExitToMenuButton.onClick.AddListener(ExitToMenu);
        if (PlayerPrefs.HasKey("CurrentLevelNumber"))
            CreatLevel(PlayerPrefs.GetInt("CurrentLevelNumber", 0));
    }

    private void Update()
    {
        _mediator.UpdateTimerTextHUD(_timer.CurrentTime());
    }

    public void CreatLevel(int levelNumber)
    {
        _currentLevelNumber = levelNumber;
        DataLevel dataLevel = _levelsDatabase.GetInfo((LevelType)levelNumber);
        EventManager.SendStartGame(dataLevel);
    }

    public void CreatLevel()
    {
        int levelNumber = _startMenuCreator.SnapScrolling.SelectedPanID;
        CreatLevel(levelNumber);
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("CurrentLevelNumber", _currentLevelNumber);
        SceneManager.LoadScene(0);
    }

    public void ExitToMenu()
    {
        PlayerPrefs.DeleteKey("CurrentLevelNumber");
        SceneManager.LoadScene(0);
    }

    private void EndGame(bool victory)
    {
        if(victory)
            Debug.Log("You win");
        else
            Debug.Log("Game over");
    }

    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.DeleteKey("CurrentLevelNumber");
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("CurrentLevelNumber");
    }
}

