using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardGame", menuName = "ScriptableObjects/CardGame", order = 1)]
public class CardGameScriptableObject : ScriptableObject
{
    private const string FileName = "CardGame";
    private static CardGameScriptableObject _instance = null;
    public static CardGameScriptableObject Instance
    {
        get
        {
            if (_instance == null) _instance = Resources.Load(FileName) as CardGameScriptableObject;
            return _instance;
        }
    }

    [SerializeField] private bool _premadeDeck = false;
    [SerializeField] private int _startingCards = 2;
    [SerializeField] private int _gameTurns = 6;
    [SerializeField] private int _turnActions = 4;

    [SerializeField] private List<int> _player1Deck = null;

    public bool PremadeDeck => _premadeDeck;
    public int StartingCards => _startingCards;
    public int GameTurns => _gameTurns;
    public int TurnActions => _turnActions;

    public List<int> Player1Deck => _player1Deck;
}
