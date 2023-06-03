using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] LevelsDatabase levelsDatabase;
    [SerializeField] StorageUIReference storageUIReference;
    [SerializeField] Mediator mediator;

    public override void InstallBindings()
    {
        BindLevelsDatabase();
        BindStorageUIReference();
        BindMediator();
        BindBoard();
        BindInputManager();
        BindGameManager();
        BindUIManager();
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

    private void BindMediator()
    {
        Container
            .Bind<IMediator>()
            .FromInstance(mediator)
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

    private void BindUIManager()
    {
        UIManager uIManager = Container
            .InstantiateComponent<UIManager>(this.gameObject);

        Container
            .Bind<UIManager>()
            .FromInstance(uIManager)
            .AsSingle();
    }
}
