using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Canvas _canvasWorldSpace;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] GameObject _gameInfo;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] TextMeshProUGUI _bombsCountText;
    public Canvas CanvasWorldSpace { get { return _canvasWorldSpace; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);

        EventManager.StartGame.AddListener(Init);
    }

    private void Init(DataLevel dataLevel)
    {
        _startMenu.SetActive(false);
        _gameInfo.SetActive(true);
        UpdateBombsCountText(dataLevel.BombsCount);
    } 

    public void UpdateBombsCountText(int bombsCount)
    {
        _bombsCountText.text = $"Bombs: {bombsCount}";
    }

    public void UpdateTimerText(float time)
    {
        _timerText.text = $"Time: {time:f0}";
    }
}
