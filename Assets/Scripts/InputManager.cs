using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float _pressTime = 0f;
    private float _maxPressTime = 0.2f;
    private bool _touchIsProcessed = false;
    private bool _isMoved = false;
    private bool _isZooming = false;


    bool isMousControl = false;
    private void Start()
    {
#if UNITY_EDITOR
        isMousControl = true;
#endif
    }
    private void Update()
    {
        if(isMousControl)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    Tile tile = hit.collider.GetComponent<Tile>();

                    if (tile != null)
                    {
                        EventManager.SendTileOpen(tile);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    Tile tile = hit.collider.GetComponent<Tile>();

                    if (tile != null)
                    {
                        EventManager.SendTileMark(tile);
                    }
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

    void OpenTile(Touch touch)
    {
        Tile tile = TileSearch(touch);
        if (tile != null)
        {
            EventManager.SendTileOpen(tile);
        }
    }

    void FlagTile(Touch touch)
    {
        Tile tile = TileSearch(touch);
        if (tile != null)
        {
            EventManager.SendTileMark(tile);
        }
    }

    Tile TileSearch(Touch touch)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null)
        {
            Tile tile = hit.collider.GetComponent<Tile>();

            if (tile != null)
                return tile;
            else
                return null;
        }
        else
            return null;
    }
}
