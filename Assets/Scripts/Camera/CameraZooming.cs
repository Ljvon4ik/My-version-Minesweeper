using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    private Camera _camera;
    private float _sensitivity = 0.01f;
    private float _maxSize = 25f;
    private float _minSize = 5f;

    private void Start()
    {
        EventManager.StartGame.AddListener(GoStartSize);
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float cameraDistance = Input.GetAxis("Mouse ScrollWheel") * _sensitivity * 100;
            _camera.orthographicSize -= cameraDistance;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
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

    private void Zooming(float distance)
    {
        float cameraDistance = distance * _sensitivity;
        _camera.orthographicSize -= cameraDistance;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
    }

    private void GoStartSize(DataLevel dataLevel)
    {
        float startSize = dataLevel.WidthBoard * Screen.height / Screen.width * 0.5f;
        _camera.orthographicSize = startSize;
    }
}