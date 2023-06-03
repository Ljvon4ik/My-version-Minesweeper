using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Board : MonoBehaviour
{
    private GameObject _tilePrefab;
    private GameObject _bombPrefab;
    private GameObject _textPrefab;
    private int _width;
    private int _height;
    private int _bombsCount;
    private int _bombsRemaining;
    private ITile[,] _tiles;
    private Canvas _canvas;
    private IMediator _mediator;

    [Inject]
    private void Construct(StorageUIReference storageUIReference, IMediator mediator)
    {
        _canvas = storageUIReference.CanvasWorldSpace;
        _mediator = mediator;
    }
    private void Awake()
    {
        EventManager.StartGameData.AddListener(Init);
        EventManager.EndGame.AddListener(OpenAllBombs);
    }

    public void Init(DataLevel dataLevel)
    {
        _tilePrefab = dataLevel.TilePrefab;
        _bombPrefab = dataLevel.BombPrefab;
        _textPrefab = dataLevel.TextPrefab;
        _width = dataLevel.WidthBoard;
        _height = dataLevel.HeightBoard;
        _bombsCount = dataLevel.BombsCount;
        _bombsRemaining = _bombsCount;
        GenerateGrid();
        _mediator.UpdateBombsCountTextHUD(_bombsRemaining);
    }
    private void GenerateGrid()
    {
        _tiles = new ITile[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject tileObject = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                ITile tile = tileObject.GetComponent<ITile>();
                tile.XPos = x;
                tile.YPos = y;
                _tiles[x, y] = tile;
            }
        }
    }

    private void PlaceBombs(int xFirstPosTail, int yFirstPosTail)
    {
        for (int i = 0; i < _bombsCount; i++)
        {
            int x = Random.Range(0, _width);
            int y = Random.Range(0, _height);

            if ((x >= xFirstPosTail - 1 && x <= xFirstPosTail + 1) && (y >= yFirstPosTail - 1 && y <= yFirstPosTail + 1))
            {
                i--;
                continue;
            }

            ITile tile = _tiles[x, y];

            if (tile.IsBomb)
            {
                i--;
            }
            else
            {
                tile.IsBomb = true;
                GameObject bombObject = Instantiate(_bombPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    private void CountAdjacentBombs()
    {
        foreach (ITile tile in _tiles)
        {
            if (!tile.IsBomb)
            {
                int adjacentBombs = 0;
                foreach (ITile adjacentTile in GetAdjacentTiles(tile))
                {
                    if (adjacentTile.IsBomb)
                        adjacentBombs++;
                }

                if (adjacentBombs > 0)
                {
                    tile.AdjacentBombCount = adjacentBombs;
                    GameObject textObject = Instantiate(_textPrefab);
                    textObject.transform.SetParent(_canvas.transform, false);
                    textObject.transform.position = new Vector3(tile.XPos, tile.YPos, 0);
                    TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
                    textMesh.text = adjacentBombs.ToString();
                }
            }
        }
    }

    public void RevalFirstTile(ITile tile)
    {
        PlaceBombs(tile.XPos, tile.YPos);
        CountAdjacentBombs();
        FloodFill(tile);
    }

    public void RevealTile(ITile tile)
    {
        if (!tile.IsRevealed && !tile.IsFlagged)
        {
            if(tile.IsBomb)
            {
                EventManager.SendEndGame(false);
                return;
            }

            if (tile.AdjacentBombCount == 0)
                FloodFill(tile);
            else
                tile.Reveal();

            CheckWin();
        }
        else if(tile.IsRevealed)
        {
            if(tile.AdjacentBombCount == CountAdjacentFlags(tile))
                EasyDigging(tile);
        }
    }

    public void FlagTile(ITile tile)
    {
        if (!tile.IsFlagged)
        {
            if (!tile.IsRevealed)
            {
                tile.Flag();
                _bombsRemaining--;
                CheckWin();
            }
        }
        else
        {
            tile.Unflag();
            _bombsRemaining++;
        }

        _mediator.UpdateBombsCountTextHUD(_bombsRemaining);
    }

    private void FloodFill(ITile tile)
    {
        if (tile.IsRevealed || tile.IsBomb)
            return;

        tile.Reveal();

        if (tile.AdjacentBombCount == 0)
        {
            foreach (ITile adjacentTiles in GetAdjacentTiles(tile))
                FloodFill(adjacentTiles);
        }
    }

    private void CheckWin()
    {
        if (_bombsRemaining == 0)
        {
            foreach (ITile tile in _tiles)
            {
                if (!tile.IsBomb && !tile.IsRevealed)
                    return;
            }

            EventManager.SendEndGame(true);
        }
    }

    private void OpenAllBombs()
    {
        foreach(ITile tile in _tiles)
        {
            if (tile.IsBomb)
                tile.Reveal();
        }
    }

    private List<ITile> GetAdjacentTiles(ITile tile)
    {
        List<ITile> adjacentTiles = new();
        for (int x = tile.XPos - 1; x <= tile.XPos + 1; x++)
        {
            for (int y = tile.YPos - 1; y <= tile.YPos + 1; y++)
            {
                if (x >= 0 && x < _width && y >= 0 && y < _height && !(x == tile.XPos && y == tile.YPos))
                    adjacentTiles.Add(_tiles[x, y]);
            }
        }
        return adjacentTiles;
    }

    private void EasyDigging(ITile tile)
    {
        foreach (ITile adjacentTiles in GetAdjacentTiles(tile))
        {
            if (!adjacentTiles.IsRevealed && !adjacentTiles.IsFlagged)
            {
                adjacentTiles.Reveal();

                if (adjacentTiles.IsBomb)
                {
                    EventManager.SendEndGame(false);
                    return;
                }

                if (adjacentTiles.AdjacentBombCount == 0)
                {
                    foreach (ITile adjacentTilesZeroBombs in GetAdjacentTiles(adjacentTiles))
                        FloodFill(adjacentTilesZeroBombs);
                }
            }
        }

        CheckWin();
    }

    private int CountAdjacentFlags(ITile tile)
    {
        int adjacentFlags = 0;

        foreach (ITile adjacentTiles in GetAdjacentTiles(tile))
        {
            if (adjacentTiles.IsFlagged)
                adjacentFlags++;
        }
        return adjacentFlags;
    }    
}
