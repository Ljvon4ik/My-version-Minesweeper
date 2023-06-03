using UnityEngine;

public class Timer : ITimer
{
    private float _startTime;
    private bool _isStopTimer = true;
    private float _gameOverTime;

    public void StartTimer()
    {
        _startTime = Time.time;
        _isStopTimer = false;
    }
    public void StopTimer()
    {
        _isStopTimer = true;
        _gameOverTime = Mathf.Round(Time.time - _startTime);
    }

    public float CurrentTime()
    {
        if (!_isStopTimer)
            return Mathf.Round(Time.time - _startTime);
        else
            return _gameOverTime;
    }
}
