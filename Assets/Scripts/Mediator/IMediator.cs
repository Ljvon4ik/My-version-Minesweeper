using UnityEngine;

public interface IMediator
{
    void UpdateBombsCountTextHUD(int bombCount);
    void UpdateTimerTextHUD(float time);
    void ShowOneWindow(GameObject window);
}
