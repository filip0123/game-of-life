using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int _gridSizeX = 0;
    [SerializeField] private int _gridSizeY = 0;
    HashSet<Vector2Int> _liveAdjecentTiles = null;

    private bool[,] _gameOfLifeGrid = null;
    private float _tickTime = 1f;
    private float _currentTickTime = 0f;

    public void Initialize()
    {
        _tickTime = GameConfigScriptableObject.Instance.TickTime;
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        _gameOfLifeGrid = new bool[_gridSizeX, _gridSizeY];
    }

    public void ResizeGrid()
    {

    }

    public void ClearGrid()
    {
    }

    public void StartSimulation()
    {
    }

    private void Tick()
    {
        HashSet<Vector2Int> liveAdjecentTilesNext = new HashSet<Vector2Int>();
        bool[,] _nextTick = new bool[_gridSizeX, _gridSizeY];

        foreach(Vector2Int tile in _liveAdjecentTiles)
        {
            _nextTick[tile.x, tile.y] = CheckNeighbours(tile.x, tile.y);
            if (_nextTick[tile.x, tile.y]) AddLiveAdjecentTiles(liveAdjecentTilesNext, tile);
        }

        _liveAdjecentTiles = liveAdjecentTilesNext;
    }

    private bool CheckNeighbours(int x, int y)
    {
        int liveCount = 0;
        for (int i = x - 1; i <= x + 1; ++i)
        {
            if (x < 0 || x > _gridSizeX) continue;
            for (int j = y - 1; j <= y + 1; ++j)
            {
                if (j < 0 || j > _gridSizeY || i == x && j == y) continue;
                else if (_gameOfLifeGrid[i, j]) ++liveCount;
            }
        }

        //RULES : live cell with 2-3 neighbours survive, dead cells with 3 neighbours become alive, other = dead
        return _gameOfLifeGrid[x, y] && liveCount == 2 || liveCount == 3; 
    }

    private void AddLiveAdjecentTiles(HashSet<Vector2Int> targetSet, Vector2Int tile)
    {
        for(int x = tile.x - 1; x <= tile.x + 1; ++x)
        {
            for (int y = tile.y - 1; y <= tile.y + 1; ++y)
            {
                if(x > 0 && y > 0 && x < _gridSizeX && y < _gridSizeY) targetSet.Add(new Vector2Int(x, y));
            }
        }
    }

    private void Update()
    {
        if (_currentTickTime > _tickTime)
        {
            Tick();
            _currentTickTime -= _tickTime;
        }
    }
}

