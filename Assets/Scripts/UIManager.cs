using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    private StorageUIReference _storageUIReference;
    IMediator _mediator;

    [Inject]
    private void Construct(StorageUIReference storageUIReference, IMediator mediator)
    {
        _storageUIReference = storageUIReference;
        _mediator = mediator;
    }

    private void Awake()
    {
        EventManager.StartGame.AddListener(delegate { _mediator.ShowOneWindow(_storageUIReference.Hud); });
    }
}
