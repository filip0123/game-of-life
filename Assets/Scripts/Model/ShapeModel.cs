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
    [SerializeField][TextArea(1, 20)] private String _tileArrangement = null;

    public ShapeType ShapeType => _shapeType;
    public string ShapeName => _shapeName;
    public Sprite CardImage => _cardImage;

    /// <summary>
    /// Transforms tileArrangment string into bool[,] by setting Os as false and Xs as true
    /// </summary>
    /// <returns></returns>
    public bool[,] getLogicalShape()
    {
        bool singleRow = _tileArrangement.IndexOf('\n') > 0;
        int sizeX = singleRow ? _tileArrangement.IndexOf('\n') : _tileArrangement.Length;

        //off by 1 because of newline chars
        if (!singleRow && (_tileArrangement.Length + 1) % (sizeX + 1) != 0) throw new Exception("Shape grid is not square!");

        int sizeY = (_tileArrangement.Length + 1) / (sizeX + 1);

        bool[,] toReturn = new bool[sizeX, sizeY];

        int x = 0;
        int y = 0;

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
            else if (character.Equals('\n'))
            {
                y++;
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

public enum ShapeType
{
    StillLife = 1,
    Oscillator = 2,
    Spaceship = 3,
}