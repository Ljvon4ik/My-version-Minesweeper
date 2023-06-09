using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;
    private float _speed = 0.1f;
    private float offset = 0.5f;
    private float _minPosX = -0.5f;//offset from tile center
    private float _minPosY = -0.5f;//offset from tile center
    private float _maxPosX;
    private float _maxPosY;
    private bool isOutOfBounds;
    private float _speedCamToBorder = 5f;
    private float _speedFix = 0.03f;
    private Vector3 _center;
    private bool _isLastActon;
    private bool _isCenter;

    bool isMousControl = false;

    private void Awake()
    {
        EventManager.StartGameData.AddListener(Init);
        EventManager.EndGame.AddListener(LastAction);
        _camera = GetComponent<Camera>();

#if UNITY_EDITOR
        isMousControl = true;
#endif
    }

    private void Update()
    {
        if(!_isLastActon)
        {
            if (isOutOfBounds)
                MoveToBorder();

            if (isMousControl)
            {
                if (Input.GetMouseButton(1))
                {
                    float horizontal = Input.GetAxis("Mouse X");
                    float vertical = Input.GetAxis("Mouse Y");
                    float ortoSize = _camera.orthographicSize;
                    _speed = ortoSize / 13f;

                    Vector3 moveDirection = new Vector3(horizontal, vertical, 0f) * _speed * 100 * Time.deltaTime;
                    transform.position -= moveDirection;
                }
                else if (Input.GetMouseButtonUp(1))
                    BoundsCheck();
            }
            else
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        float ortoSize = _camera.orthographicSize;
                        _speed = ortoSize * _speedFix;

                        Vector2 touchDeltaPosition = touch.deltaPosition;

                        Vector3 moveDirection = new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f) * _speed * Time.deltaTime;
                        transform.position -= moveDirection;
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        BoundsCheck();
                }
            }
        }
        else
        {
            if (!_isCenter)
            {
                MoveToCenter();
                CenterCheck();
            }
        }
    }

    private void Init(DataLevel dataLevel)
    {
        _maxPosX = dataLevel.WidthBoard - offset;
        _maxPosY = dataLevel.HeightBoard - offset;
        float centerX = (float)dataLevel.WidthBoard / 2 - offset;
        float centerY = (float)dataLevel.HeightBoard / 2 - offset;
        _center = new Vector3(centerX, centerY, transform.position.z);
        transform.position = _center;
    }
    private void BoundsCheck()
    {
        float xRounded = (float)Math.Round(transform.position.x, 2);
        float yRounded = (float)Math.Round(transform.position.y, 2);

        if (xRounded >= _minPosX && xRounded <= _maxPosX && yRounded >= _minPosY && yRounded <= _maxPosY)
            isOutOfBounds = false;
        else
            isOutOfBounds = true;
    }
    private void MoveToBorder()
    {
        float targetX = Mathf.Clamp(transform.position.x, _minPosX, _maxPosX);
        float targetY = Mathf.Clamp(transform.position.y, _minPosY, _maxPosY);
        Vector3 target = new Vector3(targetX, targetY, transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, _speedCamToBorder * Time.deltaTime);
        transform.position = currentPosition;

        BoundsCheck();
    }
    private void MoveToCenter()
    {
        Vector3 currentPosition = Vector3.Lerp(transform.position, _center, _speedCamToBorder * Time.deltaTime);
        transform.position = currentPosition;
    }
    private void CenterCheck()
    {
        float difference = Mathf.Abs(transform.position.sqrMagnitude - _center.sqrMagnitude);

        if (difference < 0.01f)
            _isCenter = true;
        else
            _isCenter = false;
    }
    private void LastAction()
    {
        _isLastActon = true;
    }

}