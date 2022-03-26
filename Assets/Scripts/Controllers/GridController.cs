using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private readonly int _STATE_LIVE = (int)TileState.live;
    private readonly int _STATE_LIVE_P1 = (int)TileState.playerOne;
    private readonly int _STATE_LIVE_P2 = (int) TileState.playerTwo;
    private readonly int _STATE_EMPTY = (int)TileState.empty;
    private readonly int _STATE_SELECTED = (int)TileState.selected;

    [SerializeField] GridView _gridView = null;
    HashSet<Vector2Int> _liveAdjecentTiles = null;
    HashSet<Vector2Int> _previewTiles = null;

    private int[,] _gameOfLifeGrid = null;
    private int _gridSizeX = 0;
    private int _gridSizeY = 0;

    private float _cycleTime = 1f;
    private float _currentCycleTime = 0f;
    private int _cyclesToSimulate = 0;

    private bool _gridInitialized = false;
    private bool _simulating = false;
    public bool GridInitialized => _gridInitialized;

    public System.Action OnSimulationOver = null;

    public void Initialize()
    {
        _cycleTime = GameConfigScriptableObject.Instance.TickTime;
        _liveAdjecentTiles = new HashSet<Vector2Int>();
        _previewTiles = new HashSet<Vector2Int>();
    }

    public void InitializeGrid(int sizeX, int sizeY)
    {
        _gridSizeX = sizeX;
        _gridSizeY = sizeY;
        _gameOfLifeGrid = new int[sizeX, sizeY];
        _gridView.InitializeGrid(sizeX, sizeY);
        _gridInitialized = true;

        SetSimulation();
    }

    public void ResizeGrid(int sizeX, int sizeY)
    {
        _gridSizeX = sizeX;
        _gridSizeY = sizeY;
        _gameOfLifeGrid = new int[sizeX, sizeY];
        _gridView.ResizeField(sizeX, sizeY);

        SetSimulation();
    }

    public void StartSimulation()
    {
        _simulating = true;
        _cyclesToSimulate = CardGameScriptableObject.Instance.CyclesPerTurn;
        _currentCycleTime = 0f;
    }

    private void PauseSimulation()
    {
        _simulating = false;
        OnSimulationOver?.Invoke();
    }

    public int CountLive(int handId)
    {
        int liveCount = 0;
        int tileState = CardGameScriptableObject.Instance.GetTileState(handId);

        foreach(int tile in _gameOfLifeGrid)
        {
            if (tile == tileState) liveCount++;
        }

        return liveCount;
    }

    public void SetSimulation()
    {
        _simulating = false;
        _liveAdjecentTiles.Clear();
        _currentCycleTime = 0f;

        if (GameConfigScriptableObject.Instance.DoRandomStart) RandomizeGrid();

        _gridView.SetActiveFields(ref _gameOfLifeGrid, _gridSizeX, _gridSizeY);
    }

    private void Cycle()
    {
        _cyclesToSimulate--;
        _gridView.SetCyclesLeft(_cyclesToSimulate);

        HashSet<Vector2Int> liveAdjecentTilesNext = new HashSet<Vector2Int>();
        int[,] _nextTick = new int[_gridSizeX, _gridSizeY];

        foreach(Vector2Int tile in _liveAdjecentTiles)
        {
            _nextTick[tile.x, tile.y] = CheckNeighbours(tile.x, tile.y);
            if (_nextTick[tile.x, tile.y] != 0) AddLiveAdjecentTiles(liveAdjecentTilesNext, tile);
        }

        _gameOfLifeGrid = _nextTick;

        if (_liveAdjecentTiles.SetEquals(liveAdjecentTilesNext))
        {
            PauseSimulation();
        }
        else
        {
            _liveAdjecentTiles = liveAdjecentTilesNext;
            _gridView.SetActiveFields(ref _gameOfLifeGrid, _gridSizeX, _gridSizeY);
        }
    }

    private int CheckNeighbours(int x, int y)
    {
        int liveCount = 0;
        int playerOneTiles = 0;
        int playerTwoTiles = 0;

        for (int i = x - 1; i <= x + 1; ++i)
        {
            if (i < 0 || i >= _gridSizeX) continue;
            for (int j = y - 1; j <= y + 1; ++j)
            {
                if (j < 0 || j >= _gridSizeY || i == x && j == y) continue;
                else if (_gameOfLifeGrid[i, j] != _STATE_EMPTY)
                {
                    if (_gameOfLifeGrid[i, j] == _STATE_LIVE_P1) playerOneTiles++;
                    else if (_gameOfLifeGrid[i, j] == _STATE_LIVE_P2) playerTwoTiles++;
                    ++liveCount;
                }
            }
        }

        int toReturn = _STATE_EMPTY;

        //RULES : live cell with 2-3 neighbours survive, dead cells with 3 neighbours become alive, other = dead
        if (_gameOfLifeGrid[x, y] != _STATE_EMPTY && liveCount == 2 || liveCount == 3)
        {
            if(CardGameScriptableObject.Instance.ReplaceRuleOn)
            {
                //REPLACE RULES : A > B => A  
                //MY RULE : NEUTRALS : A = B => N
                if (playerOneTiles != 0 || playerTwoTiles != 0) toReturn = playerOneTiles > playerTwoTiles ? _STATE_LIVE_P1 : _STATE_LIVE_P2;
                else toReturn = _STATE_LIVE;
            }
            else
            {
                //DELETE RULES :  A  > B => DEAD 
                //MY RULES : NEUTRALS : A = B => N 
                //                      A = 0 & B != 0 => B
                //                      A = 0 & B = 0 => N
                if (playerOneTiles != 0 && playerTwoTiles != 0) toReturn = playerTwoTiles == playerOneTiles ? _STATE_LIVE : _STATE_EMPTY;
                else if (playerOneTiles != 0 || playerTwoTiles != 0) toReturn = playerOneTiles > playerTwoTiles ? _STATE_LIVE_P1 : _STATE_LIVE_P2;
                else toReturn = _STATE_LIVE;
            }
        }

        return toReturn; 
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
                _gameOfLifeGrid[x, y] = isLive ? _STATE_LIVE : _STATE_EMPTY;
                if (isLive) AddLiveAdjecentTiles(_liveAdjecentTiles, new Vector2Int(x,y));
            }
        }
    }

    public bool CanSetShape(Vector2Int targetTile, bool[,] tileShape, int tileShapeSizeX, int tileShapeSizeY)
    {
        int gridX = 0;
        int gridY = 0;

        for (int x = 0; x < tileShapeSizeX; ++x)
        {
            gridX = targetTile.x + x;
            if (gridX == _gridSizeX) return false;
            for (int y = 0; y < tileShapeSizeY ; ++y)
            {
                gridY = targetTile.y + y;
                if (gridY == _gridSizeY) return false;
                if (tileShape[x, y])
                {
                    if (_gameOfLifeGrid[gridX, gridY] != _STATE_EMPTY && _gameOfLifeGrid[gridX, gridY] != _STATE_SELECTED) return false;
                }
            }
        }

        return true;
    }

    public void SetShape(Vector2Int targetTile, bool[,] tileShape, int tileShapeSizeX, int tileShapeSizeY, int playerId)
    {
        int gridX = 0;
        int gridY = 0;

        int playerState = CardGameScriptableObject.Instance.GetTileState(playerId);

        for (int x = 0; x < tileShapeSizeX; ++x)
        {
            gridX = targetTile.x + x;
            for (int y = 0; y < tileShapeSizeY; ++y)
            {
                gridY = targetTile.y + y;
                _gameOfLifeGrid[gridX, gridY] = tileShape[x, y] ? playerState : _gameOfLifeGrid[gridX, gridY];
                if(_gameOfLifeGrid[gridX, gridY] == playerState) AddLiveAdjecentTiles(_liveAdjecentTiles, new Vector2Int(gridX, gridY));
            }
        }

        _gridView.SetActiveFields(ref _gameOfLifeGrid, _gridSizeX, _gridSizeY);
    }

    public void PreviewShape(Vector2Int targetTile, bool[,] tileShape, int tileShapeSizeX, int tileShapeSizeY)
    {
        int gridX;
        int gridY;

        for (int x = 0; x < tileShapeSizeX; ++x)
        {
            gridX = targetTile.x + x;
            for (int y = 0; y < tileShapeSizeY; ++y)
            {
                gridY = targetTile.y + y;
                _gameOfLifeGrid[gridX, gridY] = tileShape[x, y] ? _STATE_SELECTED : _gameOfLifeGrid[gridX, gridY];
                if (tileShape[x, y]) _previewTiles.Add(new Vector2Int(gridX, gridY));
            }
        }

        _gridView.SetActiveFields(ref _gameOfLifeGrid, _gridSizeX, _gridSizeY);
    }

    public void ClearPreviews()
    {
        foreach(Vector2Int previewTile in _previewTiles)
        {
            if(_gameOfLifeGrid[previewTile.x, previewTile.y] == _STATE_SELECTED) _gameOfLifeGrid[previewTile.x, previewTile.y] = _STATE_EMPTY;
        }

        _previewTiles.Clear();

        _gridView.SetActiveFields(ref _gameOfLifeGrid, _gridSizeX, _gridSizeY);
    }

    public void ClearGrid()
    {
        ClearPreviews();

        for (int x = 0; x < _gridSizeX; ++x)
        {
            for (int y = 0; y < _gridSizeY; ++y)
            {
                _gameOfLifeGrid[x, y] = _STATE_EMPTY;
            }
        }

        _gridView.SetActiveFields(ref _gameOfLifeGrid, _gridSizeX, _gridSizeY);
    }

    private void Update()
    {
        if (_simulating)
        {
            if (_currentCycleTime > _cycleTime)
            {
                Cycle();

                if (_cyclesToSimulate == 0) PauseSimulation();
                _currentCycleTime -= _cycleTime;
            }

            _currentCycleTime += Time.deltaTime;
        }
    }
}

