using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private int _handOnTurnId = 0;
    private int _currentTurn = 0;
    private bool _gameOver = false;
    public int HandOnTurnId => _handOnTurnId;

    [SerializeField] private GridController _gridController = null;
    [SerializeField] private List<HandController> _hands = null;

    public void Initialize()
    {
        _gridController.OnSimulationOver += OnSimulationOver;
    }

    public void StartGame()
    {
        _gameOver = false;

        _handOnTurnId = 0;

        foreach(HandController hand in _hands)
        {
            hand.ResetPoints();
        }

        _hands[HandOnTurnId].StartTurn(_currentTurn);
    }

    public void ResetGame()
    {
        foreach(HandController hand in _hands)
        {
            hand.ClearGame();
        }

        _currentTurn = 0;
        _handOnTurnId = 0;
    }

    public void ChangeTurn()
    {
        if (_gameOver) return;

        _handOnTurnId++;
        _gridController.StartSimulation();
    }

    private void OnSimulationOver()
    {
        if (_handOnTurnId == _hands.Count)
        {
            _handOnTurnId = 0;
            _currentTurn++;
        }

        if (_currentTurn == CardGameScriptableObject.Instance.GameTurns)
        {
            foreach (HandController hand in _hands)
            {
                hand.AddPoints(_gridController.LiveCount);
            }
            _gameOver = true;
        }
        else
        {
            _hands[HandOnTurnId].StartTurn(_currentTurn);
        }
    }
}
