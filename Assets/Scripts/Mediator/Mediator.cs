using UnityEngine;
using Zenject;

public class Mediator : MonoBehaviour, IMediator
{
    private StorageUIReference _storageUIReference;
    private HUD _hud;
    AbstrtactWindowSwitch _windowSwitch;

    [Inject]
    private void Construct(StorageUIReference storageUIReference)
    {
        _storageUIReference = storageUIReference;
        _hud = storageUIReference.Hud.GetComponent<HUD>();
    }

    public void UpdateBombsCountTextHUD(int bombCount) => _hud.UpdateBombsCountText(bombCount);
    public void UpdateTimerTextHUD(float time) => _hud.UpdateTimerText(time);
    public void ShowOneWindow(GameObject window)
    {       
        if(_windowSwitch == null)
            _windowSwitch = new WindowSwitch(_storageUIReference.Windows);

        _windowSwitch.ShowOneWindow(window);
    }
}
