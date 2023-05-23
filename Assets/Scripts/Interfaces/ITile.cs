public interface ITile
{
    public bool IsRevealed { get; }
    public bool IsFlagged { get; }
    public bool IsBomb { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public int AdjacentBombCount { get; set; }
    public void Reveal();
    public void Flag();
    public void Unflag();
}
