using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private int _handOnTurnId = 0;
    private int _currentTurn = 0;
    private bool _gameOver = false;

    [SerializeField] private GridController _gridController = null;
    [SerializeField] private List<HandController> _hands = null;

    public System.Action OnEndGame = null;
    public int HandOnTurnId => _handOnTurnId;
    public int CurrentTurn => _currentTurn;

    public void Initialize()
    {
        _gameOver = true;

        _gridController.OnSimulationOver += OnSimulationOver;

        for(int i = 0; i < _hands.Count; ++i)
        {
            _hands[i].InitializeHand(i);
        }
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

        if (HandOnTurnId != _hands.Count) _hands[HandOnTurnId].EndTurn();

        _currentTurn = 0;
        _handOnTurnId = 0;

        StartGame();
    }

    public void ChangeTurn()
    {
        if (_gameOver || _handOnTurnId == _hands.Count) return;

        _hands[_handOnTurnId].EndTurn();
        _handOnTurnId++;

        if (_handOnTurnId == _hands.Count) _gridController.StartSimulation();
        else _hands[HandOnTurnId].StartTurn(_currentTurn);
    }

    public Dictionary<int,int> GetScores()
    {
        Dictionary<int, int> scores = new Dictionary<int,int>();
        for(int i = 0; i < _hands.Count; ++i)
        {
            scores.Add(i, _hands[i].Points);
        }

        return scores;
    }

    private void EndGame()
    {
        for (int i = 0; i < _hands.Count; ++i)
        {
            _hands[i].AddPoints(_gridController.CountLive(i));
        }

        _gameOver = true;

        OnEndGame?.Invoke();
    }

    private void OnSimulationOver()
    {
        _handOnTurnId = 0;
        _currentTurn++;

        if (_currentTurn == CardGameScriptableObject.Instance.GameTurns)
        {
            EndGame();
        }
        else
        {
            _hands[HandOnTurnId].StartTurn(_currentTurn);
        }
    }
}
