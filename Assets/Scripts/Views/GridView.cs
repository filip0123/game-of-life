using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    GridTileView _gridTilePrefab = null;
    [SerializeField] Transform _bottomLeft = null;
    [SerializeField] Transform _topRight = null;

    List<List<GridTileView>> _allTiles = null;

    private float _tileSize = 0f;
    private int _currentSizeX => _allTiles.Count;
    private int _currentSizeY => _allTiles[0].Count;

    public void InitializeGrid(int sizeX, int sizeY)
    {
        if (sizeX < 2 || sizeY < 2) throw new UnityException("Grid too small for game!");

        _gridTilePrefab = GameConfigScriptableObject.Instance.TilePrefab;

        _allTiles = new List<List<GridTileView>>();

        for (int x = 0; x < sizeX; ++x)
        {
            List<GridTileView> currentColumn = new List<GridTileView>();
            for (int y = 0; y < sizeY; ++y)
            {
                currentColumn.Add(Instantiate(_gridTilePrefab, transform));
            }
            _allTiles.Add(currentColumn);
        }

        ScaleTiles();
        PositionTiles();
    }

    public void ResizeField(int newSizeX, int newSizeY)
    {
        if (newSizeX < 2 || newSizeY < 2) throw new UnityException("Grid too small for game!");
        if (_allTiles == null) throw new UnityException("No original to resize!");

        int sizeX = _currentSizeX;
        int sizeY = _currentSizeY;

        if (newSizeX < sizeX)
        {
            for (int i = newSizeX; i < sizeX; ++i)
            {
                foreach (GridTileView tile in _allTiles[i])
                {
                    Destroy(tile.gameObject);
                }
            }
            _allTiles.RemoveRange(newSizeX, sizeX - newSizeX);
            sizeX = newSizeX;
        }

        if (newSizeY < sizeY)
        {
            foreach (List<GridTileView> column in _allTiles)
            {
                for (int i = newSizeY; i < sizeY; ++i)
                {
                    Destroy(column[i].gameObject);
                }
                column.RemoveRange(newSizeY, sizeY - newSizeY);
            }
            sizeY = newSizeY;
        }

        if (newSizeX > sizeX)
        {
            for (int x = sizeX; x < newSizeX; ++x)
            {
                List<GridTileView> currentColumn = new List<GridTileView>();
                for (int y = 0; y < sizeY; ++y)
                {
                    currentColumn.Add(Instantiate(_gridTilePrefab, transform));
                }
                _allTiles.Add(currentColumn);
            }
        }

        if (newSizeY > sizeY)
        {
            int newRows = newSizeY - sizeY;
            foreach (List<GridTileView> column in _allTiles)
            {
                for (int i = 0; i < newRows; ++i)
                {
                    column.Add(Instantiate(_gridTilePrefab, transform));
                }
            }
        }

        ScaleTiles();
        PositionTiles();
    }

    public void SetActiveFields(bool[,] logicGrid, int gridSizeX, int gridSizeY)
    {
        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                _allTiles[x][y].SetLive(logicGrid[x, y]);
            }
        }
    }

    public void Clear()
    {
        foreach (List<GridTileView> column in _allTiles)
        {
            foreach (GridTileView grid in column)
            {
                grid.SetLive(false);
            }
        }
    }

    private void ScaleTiles()
    {
        Vector2 fieldSize = _topRight.transform.position - _bottomLeft.transform.position;
        float maxTileSizeX = fieldSize.x / _currentSizeX;
        float maxTileSizeY = fieldSize.y / _currentSizeY;
        _tileSize = Mathf.Min(maxTileSizeX, maxTileSizeY);

        Vector3 tileSize3D = Vector3.one * _tileSize;

        foreach (List<GridTileView> column in _allTiles)
        {
            foreach (GridTileView grid in column)
            {
                grid.transform.localScale = tileSize3D;
            }
        }
    }

    private void PositionTiles()
    {
        Vector3 startPos = new Vector3(-_tileSize * _currentSizeX / 2f, -_tileSize * _currentSizeY / 2f); //startPos = botLeft corner of full grid

        float currentPosX = startPos.x;
        float currentPosY;

        foreach (List<GridTileView> column in _allTiles)
        {
            currentPosY = startPos.y;
            foreach (GridTileView tile in column)
            {
                tile.transform.position = new Vector3(currentPosX, currentPosY, 0f);
                currentPosY += _tileSize;
            }
            currentPosX += _tileSize;
        }
    }
}
