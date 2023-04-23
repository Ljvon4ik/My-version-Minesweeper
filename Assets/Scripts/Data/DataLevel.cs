using UnityEngine;

[CreateAssetMenu(fileName = "DataLevel", menuName = "Data/Data level")]
public class DataLevel : ScriptableObject
{
    public LevelType Type;
    [SerializeField] private int _widthBoard;
    [SerializeField] private int _heightBoard;
    [SerializeField] private int _bombsCount;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _textPrefab;

    public int BombsCount => _bombsCount;
    public int WidthBoard => _widthBoard;
    public int HeightBoard => _heightBoard;
    public GameObject TilePrefab => _tilePrefab;
    public GameObject BombPrefab => _bombPrefab;
    public GameObject TextPrefab => _textPrefab;
}
