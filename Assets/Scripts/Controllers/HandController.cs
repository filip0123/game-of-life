using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    [SerializeField] private GameObject _handContainer = null;
    [SerializeField] private DeckController _deckController = null;
    [SerializeField] private CardController _cardController = null;
    [SerializeField] private TextMeshProUGUI _actionsTxt = null;
    [SerializeField] private TextMeshProUGUI _pointsTxt = null;

    private int _points = 0;
    private int _actions = 0;
    private List<CardView> _cards = null;
    private bool _onTurn = true;

    public GameObject HandContainer => _handContainer;

    public void AddCard(CardView card)
    {
        if (_cards == null) _cards = new List<CardView>();

        card.OnPointerDownAction = null;
        card.OnPointerDownAction += () => TryPlay(card);

        _cards.Add(card);
    }

    public void ClearGame()
    {
        foreach(CardView card in _cards)
        {
            Destroy(card);
        }

        _actions = 0;
        _points = 0;
        _cards.Clear();
    }

    public void TryPlay(CardView card)
    {
        if (_onTurn && _actions >= card.Shape.Cost)
        {
            _cardController.OnCardPlaced = () => OnCardPlace(card);
            _cardController.StartDrag(card, this);
        }
    }

    public void StartTurn(int currentTurn)
    {
        _actions = CardGameScriptableObject.Instance.TurnActions;
        _actionsTxt.text = _actions.ToString();

        if(currentTurn == 0)
        {
            for (int i = 0; i < CardGameScriptableObject.Instance.StartingCards; ++i)
            {
                _deckController.Draw(this);
            }
        }

        _deckController.Draw(this);
    }

    public void OnCardPlace(CardView card)
    {
        _actions -= card.Shape.Cost;
        _actionsTxt.text = _actions.ToString();

        _cards.Remove(card);
    }

    public void EndTurn()
    {
        _onTurn = false;
    }

    public void AddPoints(int amount)
    {
        _points = _points + amount;
        _pointsTxt.text = _points.ToString();
    }

    public void ResetPoints()
    {
        _points = 0;
        _pointsTxt.text = "0";
    }
}
