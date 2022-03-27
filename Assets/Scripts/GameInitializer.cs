using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GridController _gridController = null;
    [SerializeField] UIController _UIController = null;
    [SerializeField] DeckController _deckController = null;
    [SerializeField] TurnController _turnController = null;
    [SerializeField] SoundController _soundController = null;
    private void Awake()
    {
        _soundController.Initialize();
        _gridController.Initialize();
        _deckController.Initialize();
        _turnController.Initialize();

        _UIController.Initialize();
    }
}
