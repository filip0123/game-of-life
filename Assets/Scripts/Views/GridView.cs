using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    GridTileView _gridTilePrefab = null;

    List<GridTileView> _activeTiles = null;
    List<List<GridTileView>> _allTiles = null;

    public void InitializeGrid(int sizeX, int sizeY)
    {
        if (sizeX == 0 || sizeY == 0) throw new UnityException("0 size grid!");

        _gridTilePrefab = GameConfigScriptableObject.Instance.TilePrefab;

        _allTiles = new List<List<GridTileView>>();

        for (int x = 0; x < sizeX; ++x)
        {
            List<GridTileView> currentColumn = new List<GridTileView>();
            for(int y = 0; y < sizeY; ++y)
            {
                currentColumn.Add(Instantiate(_gridTilePrefab, transform));
            }
            _allTiles.Add(currentColumn);
        }
    }

    public void ResizeField(int sizeX, int sizeY)
    {
    }

    public void SetActiveFields(bool[,] logicGrid, int gridSizeX, int gridSizeY)
    { 
        for(int x = 0; x < gridSizeX; ++x)
        {
            for(int y = 0; y < gridSizeY; ++y)
            {
                _allTiles[x][y].SetLive(logicGrid[x, y]);
            }
        }
    }

    public void Clear(int gridSizeX, int gridSizeY)
    {
        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                _allTiles[x][y].SetLive(false);
            }
        }
    }
}
