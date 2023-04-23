using UnityEngine;
public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite flagSprite;
    [SerializeField] private Sprite tileSprite;
    public bool IsRevealed { get; private set; }
    public bool IsFlagged { get; private set; }

    public int x;
    public int y;
    public bool isBomb = false;
    public int adjacentBombCount = 0;

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
