using UnityEngine;
using Zenject;

public class WindowSwitch : MonoBehaviour
{
    private GameObject[] _windows;
    private GameObject _startMenu;
    private GameObject _hud;

    [Inject]
    private void Construct(StorageUIReference storageUIReference)
    {
        _windows = storageUIReference.Windows;
        _startMenu = storageUIReference.StartMenu;
        _hud = storageUIReference.Hud;
    }

    private void Awake()
    {
        EventManager.StartGame.AddListener(delegate { ShowWindow(_hud); });
    }

    private void ShowWindow(GameObject window)
    {
        foreach (GameObject _window in _windows)
        {
            if (_window == window)
                _window.SetActive(true);
            else
                _window.SetActive(false);
        }
    }
}
