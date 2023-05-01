using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelsDatabase _levelsDatabase;
    private Board _board;
    private InputManager _inputManager;
    private Timer _timer;
    private int _currentLevelNumber;

    private void Start()
    {
        EventManager.EndGameBool.AddListener(EndGame);
        _board = this.AddComponent<Board>();
        _timer = this.AddComponent<Timer>();
        if(PlayerPrefs.HasKey("CurrentLevelNumber"))
            CreatLevel(PlayerPrefs.GetInt("CurrentLevelNumber", 0));
    }

    public void CreatLevel(int levelNumber)// запускается ивентом кнопки
    {
        _currentLevelNumber = levelNumber;
        DataLevel dataLevel = _levelsDatabase.GetInfo((LevelType)levelNumber);
        EventManager.SendStartGame(dataLevel);
        _inputManager = this.AddComponent<InputManager>();
        _inputManager.Init(_board);
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

