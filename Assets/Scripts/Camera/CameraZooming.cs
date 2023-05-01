using System;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    private Camera _camera;
    private float _sensitivity = 0.01f;
    private float _maxSize = 25f;
    private float _minSize = 5f;

    private float _initSize;
    private int _speed = 3;
    private bool _isLastAction;
    private bool _isInitSize;
    private float _endGameOffset = 1.2f;

    private float _timeLastSingleTouch = 0;
    private float _doubleClickZoomDist = 500;

    private void Awake()
    {
        EventManager.StartGame.AddListener(GoStartSize);
        EventManager.EndGame.AddListener(LastAction);
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!_isLastAction)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float cameraDistance = Input.GetAxis("Mouse ScrollWheel") * _sensitivity * 100;
                _camera.orthographicSize -= cameraDistance;
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
            }

            if (Input.GetMouseButtonDown(0))
            {
                float timeDifference = Time.time - _timeLastSingleTouch;
                _timeLastSingleTouch = Time.time;
                if (timeDifference < 0.2f)
                    Zooming(_doubleClickZoomDist);
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchStartDeltaPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchEndDeltaPos = touchOne.position - touchOne.deltaPosition;

                float distDeltaTouches = (touchStartDeltaPos - touchEndDeltaPos).magnitude;
                float currentDistTouchesPos = (touchZero.position - touchOne.position).magnitude;

                float distance = currentDistTouchesPos - distDeltaTouches;

                Zooming(distance);
            }
        }
        else
        {
            if(!_isInitSize)
            {
                ZoomingToInitSize();
                CheckSize();
            }
        }
    }

    private void Zooming(float distance)
    {
        float cameraDistance = distance * _sensitivity;
        _camera.orthographicSize -= cameraDistance;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
    }

    private void GoStartSize(DataLevel dataLevel)
    {
        _initSize = dataLevel.WidthBoard * Screen.height / Screen.width * 0.5f;
        _camera.orthographicSize = _initSize;
    }

    private void ZoomingToInitSize()
    {
        float currentSize = Mathf.Lerp(_camera.orthographicSize, _initSize * _endGameOffset, _speed * Time.deltaTime);
        _camera.orthographicSize = currentSize;
    }

    private void CheckSize()
    {
        float difference = Mathf.Abs(_camera.orthographicSize - _initSize * _endGameOffset);
        if (difference < 0.01f)
            _isInitSize = true;
        else
            _isInitSize = false;
    }
    private void LastAction()
    {
        _isLastAction = true;
    }
}