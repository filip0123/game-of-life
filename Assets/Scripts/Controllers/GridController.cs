using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int _gridSizeX = 0;
    [SerializeField] private int _gridSizeY = 0;

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
    }

    private void Update()
    {
        if(_currentTickTime > _tickTime)
        {
            Tick();
            _currentTickTime -= _tickTime;
        }
    }
}
