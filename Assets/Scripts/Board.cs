using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    private GameObject _tilePrefab;
    private GameObject _bombPrefab;
    private GameObject _textPrefab;
    private int _width;
    private int _height;
    private int _bombsCount;
    private int _bombsRemaining;
    private bool _isFirstClick = true;
    private GameObject _canvas;
    private Tile[,] _tiles;

    private void Start()
    {
        EventManager.StartGame.AddListener(Init);
        EventManager.OnTileOpen.AddListener(RevealTile);
        EventManager.OnTileMark.AddListener(FlagTile);
        _canvas = GameObject.Find("CanvasWorldSpace");
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
        _isFirstClick = true;
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        _tiles = new Tile[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject tileObject = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                Tile tile = tileObject.GetComponent<Tile>();
                tile.x = x;
                tile.y = y;
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

            // Проверяем, находится ли текущий тайл в исключаемом диапазоне
            if ((x >= xFirstPosTail - 1 && x <= xFirstPosTail + 1) && (y >= yFirstPosTail - 1 && y <= yFirstPosTail + 1))
            {
                i--;
                continue;
            }

            Tile tile = _tiles[x, y];

            if (tile.isBomb)
            {
                i--;
            }
            else
            {
                tile.isBomb = true;
                GameObject bombObject = Instantiate(_bombPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    private void CountAdjacentBombs()
    {
        foreach (Tile tile in _tiles)
        {
            if (!tile.isBomb)
            {
                int adjacentBombs = 0;
                foreach (Tile adjacentTile in GetAdjacentTiles(tile))
                {
                    if (adjacentTile.isBomb)
                    {
                        adjacentBombs++;
                    }
                }

                if (adjacentBombs > 0)
                {
                    tile.adjacentBombCount = adjacentBombs;
                    GameObject textObject = Instantiate(_textPrefab);
                    textObject.transform.SetParent(_canvas.transform, false);
                    textObject.transform.position = new Vector3(tile.x, tile.y, 0);
                    TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
                    textMesh.text = adjacentBombs.ToString();
                }
            }
        }
    }

    private void RevealTile(Tile tile)
    {
        if (_isFirstClick)
        {
            PlaceBombs(tile.x, tile.y);
            CountAdjacentBombs();
            FloodFill(tile);
            _isFirstClick = false;
        }
        else
        {
            if (!tile.IsRevealed && !tile.IsFlagged)
            {
                if (tile.isBomb)
                {
                    tile.Reveal();
                    EventManager.SendEndGame(false);
                    return;
                }
                else if (tile.adjacentBombCount == 0)
                    FloodFill(tile);
                else
                    tile.Reveal();

                CheckWin();
            }
        }
    }

    private void FlagTile(Tile tile)
    {
        if (!tile.IsFlagged)
        {
            if (!tile.IsRevealed && _bombsRemaining > 0)
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
    }

    private void FloodFill(Tile tile)
    {
        if (tile.IsRevealed || tile.isBomb)
            return;

        tile.Reveal();

        if (tile.adjacentBombCount == 0)
        {
            foreach (Tile adjacentTiles in GetAdjacentTiles(tile))
            {
                FloodFill(adjacentTiles);
            }
        }
    }

    private void CheckWin()
    {
        if (_bombsRemaining == 0)
        {
            foreach (Tile tile in _tiles)
            {
                if (!tile.isBomb && !tile.IsRevealed)
                {
                    return;
                }
            }

            EventManager.SendEndGame(true);
        }
    }

    private List<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> adjacentTiles = new List<Tile>();
        for (int x = tile.x - 1; x <= tile.x + 1; x++)
        {
            for (int y = tile.y - 1; y <= tile.y + 1; y++)
            {
                if (x >= 0 && x < _width && y >= 0 && y < _height && !(x == tile.x && y == tile.y))
                {
                    adjacentTiles.Add(_tiles[x, y]);
                }
            }
        }
        return adjacentTiles;
    }
}
