using TMPro;
using UnityEngine;

public class InfoLevelPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameTxt;
    [SerializeField] private TextMeshProUGUI boardSizeTxt;
    [SerializeField] private TextMeshProUGUI bombsCountTxt;

    public void Init(DataLevel dataLevel)
    {
        levelNameTxt.text = dataLevel.name;
        boardSizeTxt.text = $"Size: {dataLevel.WidthBoard} * {dataLevel.HeightBoard}";
        bombsCountTxt.text = $"Bombs: {dataLevel.BombsCount}";
    }
}
