using UnityEngine.Events;

public class EventManager
{
    public static UnityEvent OnFirstClick = new UnityEvent();
    public static UnityEvent<DataLevel> StartGameData = new UnityEvent<DataLevel>();
    public static UnityEvent StartGame = new UnityEvent();
    public static UnityEvent<bool> EndGameBool = new UnityEvent<bool>();
    public static UnityEvent EndGame = new UnityEvent();

    public static void SendFirstClick()
    {
        OnFirstClick.Invoke();
    }

    public static void SendStartGame(DataLevel dataLevel)
    {
        StartGameData.Invoke(dataLevel);
        StartGame.Invoke();
    }
    public static void SendEndGame(bool victory)
    {
        EndGameBool.Invoke(victory);
        EndGame.Invoke();
    }
}
