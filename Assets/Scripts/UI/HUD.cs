using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _bombsCountText;

    public void UpdateBombsCountText(int bombsCount)
    {
        _bombsCountText.text = $"Bombs: {bombsCount}";
    }

    public void UpdateTimerText(float time)
    {
        _timerText.text = $"Time: {time:f0}";
    }
}
