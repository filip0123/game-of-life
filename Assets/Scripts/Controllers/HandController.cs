using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    [SerializeField] private DeckController _deckController = null;
    [SerializeField] private CardController _cardController = null;
    [SerializeField] private TextMeshProUGUI _actionsTxt = null;

    private int _handId = 0;
    private int _actions = 0;
    private List<CardView> _cards = null;
    private bool _onTurn = true;

    public void Initialize(int handId)
    {
        _handId = handId;
    }

    public void AddCard(CardView card)
    {
        if (_cards == null) _cards = new List<CardView>();

        card.OnPointerDownAction = null;
        card.OnPointerDownAction += () => TryPlay(card);

        _cards.Add(card);
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
