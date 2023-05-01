using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _startTime;
    private float _elapsedTime;
    private bool _isStopTimer = true;
    public float GameEndTime { get; private set; }

    private void Awake()
    {
        EventManager.OnFirstClick.AddListener(StartTimer);
        EventManager.EndGame.AddListener(StopTimer);
    }

    void Update()
    {
        if (!_isStopTimer)
        {
            _elapsedTime = Mathf.Round(Time.time - _startTime);
            UIManager.Instance.UpdateTimerText(_elapsedTime);
        }
    }
    public void StartTimer()
    {
        _startTime = Time.time;
        _isStopTimer = false;
    }
    private void StopTimer()
    {
        _isStopTimer = true;
        GameEndTime = _elapsedTime;
    }
}
