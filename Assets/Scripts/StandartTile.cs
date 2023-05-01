using UnityEngine;
public class StandartTile : MonoBehaviour, ITile
{
    public bool IsRevealed { get; private set; }
    public bool IsFlagged { get; private set; }
    public bool IsBomb { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public int AdjacentBombCount { get; set; }

    [SerializeField] private Sprite flagSprite;
    [SerializeField] private Sprite tileSprite;

    public void Reveal()
    {
        IsRevealed = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Flag()
    {
        IsFlagged = true;
        GetComponent<SpriteRenderer>().sprite = flagSprite;
    }

    public void Unflag()
    {
        IsFlagged = false;
        GetComponent<SpriteRenderer>().sprite = tileSprite;
    }
}
