using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] GridView _gridView = null;
    HashSet<Vector2Int> _liveAdjecentTiles = null;

    private bool[,] _gameOfLifeGrid = null;
    private int _gridSizeX = 0;
    private int _gridSizeY = 0;

    private float _tickTime = 1f;
    private float _currentTickTime = 0f;

    private bool _gridInitialized = false;
    private bool _simulating = false;
    public bool GridInitialized => _gridInitialized;

    public void Initialize()
    {
        _tickTime = GameConfigScriptableObject.Instance.TickTime;
        _liveAdjecentTiles = new HashSet<Vector2Int>();
    }

    public void InitializeGrid(int sizeX, int sizeY)
    {
        _gridSizeX = sizeX;
        _gridSizeY = sizeY;
        _gameOfLifeGrid = new bool[sizeX, sizeY];
        _gridView.InitializeGrid(sizeX, sizeY);
        _gridInitialized = true;

        if (GameConfigScriptableObject.Instance.DoRandomStart) RandomizeGrid();

        _gridView.SetActiveFields(_gameOfLifeGrid,sizeX,sizeY);
    }

    public void ResizeGrid(int sizeX, int sizeY)
    {
        _gridSizeX = sizeX;
        _gridSizeY = sizeY;
        _gameOfLifeGrid = new bool[sizeX, sizeY];
        _gridView.ResizeField(sizeX, sizeY);

        if (GameConfigScriptableObject.Instance.DoRandomStart) RandomizeGrid();

        _gridView.SetActiveFields(_gameOfLifeGrid, sizeX, sizeY);
    }

    public void ClearGrid()
    {
        _gridView.Clear();
    }

    public void StartSimulation()
    {
        _simulating = true;
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

        _gameOfLifeGrid = _nextTick;

        if (_liveAdjecentTiles.SetEquals(liveAdjecentTilesNext))
        {
            _simulating = false;
        }
        else
        {
            _liveAdjecentTiles = liveAdjecentTilesNext;
            _gridView.SetActiveFields(_gameOfLifeGrid, _gridSizeX, _gridSizeY);
        }
    }

    private bool CheckNeighbours(int x, int y)
    {
        int liveCount = 0;
        for (int i = x - 1; i <= x + 1; ++i)
        {
            if (i < 0 || i >= _gridSizeX) continue;
            for (int j = y - 1; j <= y + 1; ++j)
            {
                if (j < 0 || j >= _gridSizeY || i == x && j == y) continue;
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
    private void RandomizeGrid()
    {
        int randomChance = GameConfigScriptableObject.Instance.RandomPercentage;
        
        for(int x = 0; x < _gridSizeX; ++x)
        {
            for(int y = 0; y < _gridSizeY; ++y)
            {
                bool isLive = Random.Range(0, 100) < randomChance;
                _gameOfLifeGrid[x, y] = isLive;
                if (isLive) AddLiveAdjecentTiles(_liveAdjecentTiles, new Vector2Int(x,y));
            }
        }
    }

    private void Update()
    {
        if (_simulating)
        {
            if (_currentTickTime > _tickTime)
            {
                Tick();
                _currentTickTime -= _tickTime;
            }
            else
            {
                _currentTickTime += Time.deltaTime;
            }
        }
    }
}

