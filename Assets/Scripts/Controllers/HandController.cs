using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    private static readonly int _MATERIAL_COLOR_HASH = Shader.PropertyToID("_Color");

    [SerializeField] private GameObject _handContainer = null;
    [SerializeField] private DeckController _deckController = null;
    [SerializeField] private CardController _cardController = null;
    [SerializeField] private TextMeshProUGUI _actionsTxt = null;

    private int _handId = 0;
    private int _points = 0;
    private int _actions = 0;
    private List<CardView> _cards = null;
    private bool _onTurn = true;
    public GameObject HandContainer => _handContainer;
    public int Points => _points;

    public void InitializeHand(int handId)
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

    public void ClearGame()
    {
        if (_cards == null) return;

        foreach(CardView card in _cards)
        {
            Destroy(card.gameObject);
        }

        _actions = 0;
        _points = 0;
        _cards.Clear();
    }

    public void TryPlay(CardView card)
    {
        if (_onTurn && _actions >= card.Shape.Cost)
        {
            _cardController.OnCardPlaced = () =>
            {
                OnCardPlace(card);
                SoundController.Instance.PlayClip((int)Sound.Button);
            };

            _cardController.StartDrag(card, _handId);
            SoundController.Instance.PlayClip((int)Sound.Button);
        }
        else
        {
            SoundController.Instance.PlayClip((int)Sound.Error);
        }
    }

    public void StartTurn(int currentTurn)
    {
        gameObject.SetActive(true);
        _onTurn = true;
        _actions = CardGameScriptableObject.Instance.TurnActions;
        _actionsTxt.text = _actions.ToString();

        if(currentTurn == 0)
        {
            for (int i = 0; i < CardGameScriptableObject.Instance.StartingCards; ++i)
            {
                _deckController.Draw(this, _handId);
            }
        }

        _deckController.Draw(this, _handId);
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
        gameObject.SetActive(false);
    }

    public void AddPoints(int amount)
    {
        _points = _points + amount;
    }

    public void ResetPoints()
    {
        _points = 0;
    }
}
