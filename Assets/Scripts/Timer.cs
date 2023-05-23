using UnityEngine;
using Zenject;

public class Timer : MonoBehaviour
{
    private float _startTime;
    private float _elapsedTime;
    private bool _isStopTimer = true;
    public float GameEndTime { get; private set; }
    private HUD _hud;

    [Inject]
    private void Construct(StorageUIReference storageUIReference)
    {
        _hud = storageUIReference.Hud.GetComponent<HUD>();
    }

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
            _hud.UpdateTimerText(_elapsedTime);
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
