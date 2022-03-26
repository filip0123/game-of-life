using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private bool _replaceRuleOn = false;
    [SerializeField] private int _playerCount = 2;
    [SerializeField] private bool _premadeDeck = false;
    [SerializeField] private int _startingCards = 2;
    [SerializeField] private int _gameTurns = 6;
    [SerializeField] private int _turnActions = 4;
    [SerializeField] private int _cyclesPerTurn = 100;
    [SerializeField] private DeckModel[] _decks = null;
    [SerializeField] private Material[] _playerTileMaterials = null;

    [SerializeField] private ShapeTypeCostModel[] _shapeTypeCosts = null;

    public bool ReplaceRuleOn => _replaceRuleOn;
    public int PlayerCount => _playerCount;
    public bool PremadeDeck => _premadeDeck;
    public int StartingCards => _startingCards;
    public int GameTurns => _gameTurns;
    public int TurnActions => _turnActions;
    public int CyclesPerTurn => _cyclesPerTurn;
    public DeckModel[] Decks => _decks;
    public Material[] PlayerTileMaterials => _playerTileMaterials;

    public int GetCost(ShapeType type)
    {
        return _shapeTypeCosts.FirstOrDefault(x => x._shapeType == type)._actionCost;
    }

    public int GetTileState(int playerId)
    {
        const int playerStateIncrement = 10;
        return playerId + playerStateIncrement;
    }
}
