using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private LevelsDatabase _levelsDatabase;
    private int _currentLevelNumber;
    private StorageUIReference _storageUIReference;

    [Inject]
    private void Construct(LevelsDatabase levelsDatabase, StorageUIReference storageUIReference)
    {
        _levelsDatabase = levelsDatabase;
        _storageUIReference = storageUIReference;
    }

    private void Awake()
    {
        EventManager.EndGameBool.AddListener(EndGame);
    }
    private void Start()
    {
        _storageUIReference.RestartButton.onClick.AddListener(Restart);
        _storageUIReference.ExitToMenuButton.onClick.AddListener(ExitToMenu);
        if (PlayerPrefs.HasKey("CurrentLevelNumber"))
            CreatLevel(PlayerPrefs.GetInt("CurrentLevelNumber", 0));
    }

    public void CreatLevel(int levelNumber)
    {
        _currentLevelNumber = levelNumber;
        DataLevel dataLevel = _levelsDatabase.GetInfo((LevelType)levelNumber);
        EventManager.SendStartGame(dataLevel);
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

