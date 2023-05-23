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


    public Canvas CanvasWorldSpace => _canvasWorldSpace;
    public GameObject StartMenu => _startMenu;
    public GameObject Hud => _hud;
    public Button RestartButton => _restartButton;
    public Button ExitToMenuButton => _exitToMenuButton;
    public Canvas Canvas => _canvas;
    public GameObject[] Windows => _windows;

    //private void Awake()
    //{
    //    _windows = new GameObject[_canvas.transform.childCount];

    //    for (int i = 0; i < _windows.Length; i++)
    //    {
    //        _windows[i] = _canvas.transform.GetChild(i).gameObject;
    //    }
    //}
}
