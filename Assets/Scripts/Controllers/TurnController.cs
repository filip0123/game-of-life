using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private static TurnController _instance = null;
    public static TurnController Instance => _instance;

    private int _handOnTurnId = 0;
    private int _currentTurn = 0;
    public int HandOnTurnId => _handOnTurnId;

    [SerializeField] private List<HandController> _hands = null;


    public void Initialize()
    {
        _instance = this;
    }

    public void ChangeTurn()
    {
        _handOnTurnId++;
        if (_handOnTurnId == _hands.Count)
        {
            _handOnTurnId = 0;
            _currentTurn++;
        }
        _hands[HandOnTurnId].StartTurn(_currentTurn);
    }
}
