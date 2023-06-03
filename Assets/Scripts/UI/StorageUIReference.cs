using UnityEngine;
using UnityEngine.UI;

public class StorageUIReference : MonoBehaviour
{
    [SerializeField] private Canvas _canvasWorldSpace;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitToMenuButton;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject[] _windows;
    [SerializeField] private Button _startButton;

    public Canvas CanvasWorldSpace => _canvasWorldSpace;
    public GameObject StartMenu => _startMenu;
    public GameObject Hud => _hud;
    public Button RestartButton => _restartButton;
    public Button ExitToMenuButton => _exitToMenuButton;
    public Canvas Canvas => _canvas;
    public GameObject[] Windows => _windows;
    public Button StartButton => _startButton;
}
