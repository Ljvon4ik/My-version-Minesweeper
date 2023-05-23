using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] LevelsDatabase levelsDatabase;
    [SerializeField] StorageUIReference storageUIReference;

    public override void InstallBindings()
    {
        BindLevelsDatabase();
        BindStorageUIReference();
        BindBoard();
        BindInputManager();
        BindTimer();
        BindGameManager();
        BindWindowSwitch();
    }

    private void BindLevelsDatabase()
    {
        Container
            .Bind<LevelsDatabase>()
            .FromInstance(levelsDatabase)
            .AsSingle();
    }
    private void BindStorageUIReference()
    {
        Container
            .Bind<StorageUIReference>()
            .FromInstance(storageUIReference)
            .AsSingle();
    }

    private void BindGameManager()
    {
        GameManager gameManager = Container
            .InstantiateComponent<GameManager>(this.gameObject);

        Container
            .Bind<GameManager>()
            .FromInstance(gameManager)
            .AsSingle();
    }

    private void BindBoard()
    {
        Board board = Container
            .InstantiateComponent<Board>(this.gameObject);

        Container
            .Bind<Board>()
            .FromInstance(board)
            .AsSingle();
    }

    private void BindInputManager()
    {
        InputManager inputManager = Container
            .InstantiateComponent<InputManager>(this.gameObject);

        Container
            .Bind<InputManager>()
            .FromInstance(inputManager)
            .AsSingle();
    }

    private void BindTimer()
    {
        Timer timer = Container
            .InstantiateComponent<Timer>(this.gameObject);

        Container
            .Bind<Timer>()
            .FromInstance(timer)
            .AsSingle();
    }

    private void BindWindowSwitch()
    {
        WindowSwitch windowSwitch = Container
            .InstantiateComponent<WindowSwitch>(this.gameObject);

        Container
            .Bind<WindowSwitch>()
            .FromInstance(windowSwitch)
            .AsSingle();
    }
}
