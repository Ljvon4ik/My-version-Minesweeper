using UnityEngine;

public class WindowSwitch : AbstrtactWindowSwitch
{
    public WindowSwitch(GameObject[] windows) : base(windows) {}
}

public abstract class AbstrtactWindowSwitch
{
    private GameObject[] _windows;
    public AbstrtactWindowSwitch(GameObject[] windows)
    {
        _windows = windows;
    }

    public void ShowOneWindow(GameObject window)
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