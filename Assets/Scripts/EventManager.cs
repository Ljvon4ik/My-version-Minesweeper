using UnityEngine.Events;

public class EventManager
{
    public static UnityEvent<Tile> OnTileOpen = new UnityEvent<Tile>();
    public static UnityEvent<Tile> OnTileMark = new UnityEvent<Tile>();
    public static UnityEvent<DataLevel> StartGame = new UnityEvent<DataLevel>();
    public static UnityEvent<bool> EndGame = new UnityEvent<bool>();

    public static void SendTileOpen(Tile tile)
    {
        OnTileOpen.Invoke(tile);
    }

    public static void SendTileMark(Tile tile)
    {
        OnTileMark.Invoke(tile);
    }
    public static void SendStartGame(DataLevel dataLevel)
    {
        StartGame.Invoke(dataLevel);
    }
    public static void SendEndGame(bool victory)
    {
        EndGame.Invoke(victory);
    }
}
