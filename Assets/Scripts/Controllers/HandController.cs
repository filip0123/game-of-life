using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    [SerializeField] private DeckController _deckController = null;
    [SerializeField] private CardController _cardController = null;
    [SerializeField] private TextMeshProUGUI _actionsTxt = null;

    private int _points = 0;
    private int _actions = 0;
    private List<CardView> _cards = null;
    private bool _onTurn = true;

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
        _cardController.StartDrag(card);
    }

    public void StartTurn(int currentTurn)
    {
        if(currentTurn == 0)
        {
            for (int i = 0; i < CardGameScriptableObject.Instance.StartingCards; ++i)
            {
                _deckController.Draw(this);
            }
        }

        _deckController.Draw(this);
    }

}
