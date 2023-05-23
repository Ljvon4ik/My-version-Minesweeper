using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    private int _panCount;
    public float _panOffset = 40f;
    public float _scaleOffset = 8f;
    public float _scaleSpeed = 10f;
    private GameObject[] _panels;
    private Vector2[] _panelsPos;
    private Vector2[] _panelsScale;
    private RectTransform _contentRect;
    private ScrollRect _scrollRect;
    private GameObject _content;
    public int _selectedPanID { get; private set; }

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _content = transform.GetChild(0).transform.GetChild(0).gameObject;// default content layout when creating
        _contentRect = _content.GetComponent<RectTransform>();
        _panCount = _content.transform.childCount;
        _panels = new GameObject[_panCount];
        _panelsPos = new Vector2[_panCount];
        _panelsScale = new Vector2[_panCount];
        for (int i = 0; i < _panCount; i++)
        {
            _panels[i] = _content.transform.GetChild(i).gameObject;
            _panelsPos[i] = -_panels[i].transform.localPosition;
        }
        StartCoroutine(ScalePanelsAnimation());

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _scrollRect.inertia = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ScalePanels();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _scrollRect.inertia = false;

        float nearstPos = float.MaxValue;

        for (int i = 0; i < _panCount; i++)
        {
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _panelsPos[i].x);
            if (distance < nearstPos)
            {
                nearstPos = distance;
                _selectedPanID = i;
            }
        }
        StartCoroutine(SnapScrollingAnimation(_panelsPos[_selectedPanID]));
        StartCoroutine(ScalePanelsAnimation());
    }

    private IEnumerator SnapScrollingAnimation(Vector2 destination)
    {
        float time = 0.2f;
        Vector2 startPosition = _contentRect.anchoredPosition;
        float currentTime = 0f;
        while (currentTime < time)
        {
            _contentRect.anchoredPosition = Vector2.Lerp(startPosition, destination, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _contentRect.anchoredPosition = destination;
    }

    private IEnumerator ScalePanelsAnimation()
    {
        float time = 0.5f;
        float currentTime = 0f;
        while (currentTime < time)
        {
            ScalePanels();
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ScalePanels()
    {
        for (int i = 0; i < _panCount; i++)
        {
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _panelsPos[i].x);
            float scale = Mathf.Clamp(1 / (distance / _panOffset) * _scaleOffset, 0.5f, 1f);
            _panelsScale[i].x = Mathf.Lerp(_panels[i].transform.localScale.x, scale, _scaleSpeed * Time.deltaTime);
            _panelsScale[i].y = Mathf.Lerp(_panels[i].transform.localScale.y, scale, _scaleSpeed * Time.deltaTime);
            _panels[i].transform.localScale = _panelsScale[i];
        }
    }

}
