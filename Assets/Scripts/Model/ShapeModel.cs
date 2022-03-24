using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShapeModel
{
    [SerializeField] private ShapeType _shapeType;
    [SerializeField] private string _shapeName = null;
    [SerializeField] private Sprite _cardImage = null;
    [SerializeField] [TextArea(1, 20)] private String _tileArrangement = null;

    private bool[,] _logicalTileArrangement = null;
    private int _sizeX = 0;
    private int _sizeY = 0;

    public bool[,] LogicalTileArrangement
    {
        get
        {
            if (_logicalTileArrangement == null) _logicalTileArrangement = getLogicalTileArrangement();
            return _logicalTileArrangement;
        }
    }

    public int SizeX => _sizeX;
    public int SizeY => _sizeY;

    public ShapeType ShapeType => _shapeType;
    public string ShapeName => _shapeName;
    public Sprite CardImage => _cardImage;




    /// <summary>
    /// Transforms tileArrangment string into bool[,] by setting Os as false and Xs as true
    /// </summary>
    /// <returns></returns>
    private bool[,] getLogicalTileArrangement()
    {
        bool singleRow = _tileArrangement.IndexOf('\n') > 0;
        _sizeX = singleRow ? _tileArrangement.IndexOf('\n') : _tileArrangement.Length;

        //off by 1 because of newline chars
        if (!singleRow && (_tileArrangement.Length + 1) % (_sizeX + 1) != 0) throw new Exception("Shape grid is not square!");

        _sizeY = (_tileArrangement.Length + 1) / (_sizeX + 1);

        bool[,] toReturn = new bool[_sizeX, _sizeY];

        int x = 0;
        int y = _sizeY - 1;

        foreach (char character in _tileArrangement)
        {
            if (character.Equals('O'))
            {
                toReturn[x, y] = false;
            }
            else if (character.Equals('X'))
            {
                toReturn[x, y] = true;
            }
            
            if (character.Equals('\n'))
            {
                y--;
                x = 0;
            }
            else
            {
                x++;
            }
        }

        return toReturn;
    }
}