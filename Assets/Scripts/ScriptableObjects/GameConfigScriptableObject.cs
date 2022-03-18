using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfigScriptableObject : ScriptableObject
{
    private const string FileName = "GameConfig";
    private static GameConfigScriptableObject _instance = null;
    public static GameConfigScriptableObject Instance
    {
        get
        {
            if (_instance == null) _instance = Resources.Load(FileName) as GameConfigScriptableObject;
            return _instance;
        }
    }

    [SerializeField] private GridTileView _tilePrefab = null;
    [SerializeField] private float _tickTime = 0f;

    [SerializeField] private bool _doRandomStart = false;
    [SerializeField] private int _randomPercentage = 0;

    public GridTileView TilePrefab => _tilePrefab;
    public float TickTime => _tickTime;
    public bool DoRandomStart => _doRandomStart;
    public int RandomPercentage => _randomPercentage;
}