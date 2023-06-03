using UnityEngine;
using Zenject;

public class StartMenuCreator : MonoBehaviour
{
    [SerializeField] private GameObject _scrollView;
    [SerializeField] private GameObject _panPref;
    private LevelsDatabase _levelsDatabase;
    public SnapScrolling SnapScrolling { get; private set; }

    [Inject]
    private void Construct(LevelsDatabase levelsDatabase)
    {
        _levelsDatabase = levelsDatabase;
    }

    private void Start()
    {
        SpawnPanPref();
        SnapScrolling = _scrollView.AddComponent<SnapScrolling>();
    }

    private void SpawnPanPref()
    {
        GameObject _content = _scrollView.transform.GetChild(0).transform.GetChild(0).gameObject;// default content layout when creating
        int _panCount = LevelType.GetNames(typeof(LevelType)).Length;
        GameObject[] _panels = new GameObject[_panCount];
        float _panOffset = 40f;

        for (int i = 0; i < _panCount; i++)
        {
            _panels[i] = Instantiate(_panPref, _content.transform, false);
            _panels[i].GetComponent<InfoLevelPanel>().Init(_levelsDatabase.GetInfo((LevelType)i));
            if (i == 0) continue;
            Vector2 pos = new Vector2(_panels[i - 1].transform.localPosition.x + _panPref.GetComponent<RectTransform>().sizeDelta.x
                + _panOffset, _panels[i].transform.localPosition.y);
            _panels[i].transform.localPosition = pos;
        }
    }
}
