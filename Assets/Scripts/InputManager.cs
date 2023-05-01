using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float _pressTime = 0f;
    private float _maxPressTime = 0.2f;
    private bool _touchIsProcessed = false;
    private bool _isMoved = false;
    private bool _isZooming = false;
    private bool _isFirstClick = true;
    private bool _isDisableInput;

    bool isMouseControl = false;

    private Board _board;
    private bool _isInit;
    private void Awake()
    {
        EventManager.EndGame.AddListener(DisableInput);

#if UNITY_EDITOR
        isMouseControl = true;
#endif
    }

    public void Init(Board board)
    {
        _board = board;
        _isFirstClick = true;
        _isDisableInput = false;
        _isInit = true;
    }
    private void Update()
    {
        if(!_isDisableInput && _isInit)
        {
            if (isMouseControl)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                    if (hit.collider != null)
                    {
                        ITile tile = hit.collider.GetComponent<ITile>();

                        if (tile != null)
                        {
                            if (!_isFirstClick)
                                _board.RevealTile(tile);
                            else
                            {
                                EventManager.SendFirstClick();
                                _board.RevalFirstTile(tile);
                                _isFirstClick = false;
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(1) && !_isFirstClick)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                    if (hit.collider != null)
                    {
                        ITile tile = hit.collider.GetComponent<ITile>();

                        if (tile != null)
                            _board.FlagTile(tile);
                    }
                }
            }
            else
            {
                if (Input.touchCount == 0)
                {
                    _isZooming = false;
                    _isMoved = false;
                }
                else if (Input.touchCount == 2 && !_isZooming)
                    _isZooming = true;
                else if (Input.touchCount == 1 && !_isZooming && !_isMoved)
                {
                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            _touchIsProcessed = false;
                            _pressTime = 0;
                            break;

                        case TouchPhase.Moved:
                            if (touch.deltaPosition.magnitude > 5f)
                            {
                                _touchIsProcessed = true;
                                _isMoved = true;
                            }
                            break;

                        case TouchPhase.Stationary:
                            _pressTime += Time.deltaTime;
                            break;

                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            if (_pressTime < _maxPressTime)
                            {
                                _touchIsProcessed = true;
                                OpenTile(touch);
                            }
                            break;
                    }

                    if (!_isMoved && !_touchIsProcessed && _pressTime >= _maxPressTime)
                    {
                        _touchIsProcessed = true;
                        FlagTile(touch);
                    }
                }
            }
        }
    }

    void OpenTile(Touch touch)
    {
        ITile tile = TileSearch(touch);
        if (tile != null)
        {
            if (!_isFirstClick)
                _board.RevealTile(tile);
            else
            {
                EventManager.SendFirstClick();
                _board.RevalFirstTile(tile);
                _isFirstClick = false;
            }
        }
    }

    void FlagTile(Touch touch)
    {
        ITile tile = TileSearch(touch);
        if (tile != null && !_isFirstClick)
            _board.FlagTile(tile);
    }

    ITile TileSearch(Touch touch)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null)
        {
            ITile tile = hit.collider.GetComponent<ITile>();

            if (tile != null)
                return tile;
            else
                return null;
        }
        else
            return null;
    }

    private void DisableInput()
    {
        _isDisableInput = true;
    }
}
