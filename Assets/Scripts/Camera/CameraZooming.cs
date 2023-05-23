using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    private Camera _camera;
    private float _sensitivity = 0.01f;
    private float _maxSize = 25f;
    private float _minSize = 5f;

    private float _initSize;
    private float _initSizeSpeed = 3f;
    private bool _isLastAction;
    private bool _isInitSize;
    private float _endGameOffset = 1.2f;

    private float _timeLastSingleTouch = 0;

    private bool _isDoubleClick;
    private float _doubleClickSize;
    private float _doubleClickSpeed = 3f;
    private float _positioningPrecision = 0.2f;


    private void Awake()
    {
        EventManager.StartGameData.AddListener(Init);
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
                if (timeDifference < 0.2f && _camera.orthographicSize > _minSize + _positioningPrecision)
                {
                    _doubleClickSize = Mathf.Clamp(_camera.orthographicSize * 0.75f, _minSize, _maxSize);
                    _isDoubleClick = true;
                }
            }

            if (_isDoubleClick)
            {
                ZoomingToSize(_doubleClickSize, _doubleClickSpeed);
                if (CheckSize(_doubleClickSize, _positioningPrecision))
                    _isDoubleClick = false;
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
                ZoomingToSize(_initSize * _endGameOffset, _initSizeSpeed);
                if(CheckSize(_initSize * _endGameOffset, 0.1f))
                    _isInitSize = true;
            }
        }
    }

    private void Zooming(float distance)
    {
        float cameraDistance = distance * _sensitivity;
        _camera.orthographicSize -= cameraDistance;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
    }

    private void Init(DataLevel dataLevel)
    {
        _initSize = dataLevel.WidthBoard * Screen.height / Screen.width * 0.5f;
        _camera.orthographicSize = _initSize;
    }

    private void ZoomingToSize(float targetSize, float speed)
    {
        float currentSize = Mathf.Lerp(_camera.orthographicSize, targetSize, speed * Time.deltaTime);
        _camera.orthographicSize = currentSize;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
    }

    private bool CheckSize(float targetSize, float precision)
    {
        float difference = Mathf.Abs(_camera.orthographicSize - targetSize);

        if (difference < precision)
            return true;
        else
            return false;
    }
    private void LastAction()
    {
        _isLastAction = true;
    }
}