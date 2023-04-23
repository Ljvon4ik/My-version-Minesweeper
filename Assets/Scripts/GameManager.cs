using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelsDatabase _levelsDatabase;
    private Board _board;

    private void Start()
    {
        EventManager.EndGame.AddListener(EndGame);
        _board = this.AddComponent<Board>();
        this.AddComponent<InputManager>();
    }

    public void CreatLevel(int level)// запускается ивентом кнопки
    {
        DataLevel dataLevel = _levelsDatabase.GetInfo((LevelType)level);
        EventManager.SendStartGame(dataLevel);
    }

    private void EndGame(bool victory)
    {
        if(victory)
            Debug.Log("You win");
        else
            Debug.Log("Game over");
    }
}

