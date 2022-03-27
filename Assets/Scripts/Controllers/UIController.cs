using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputFieldSizeX = null;
    [SerializeField] TextMeshProUGUI _inputFieldSizeXPlaceholder = null;
    [SerializeField] TMP_InputField _inputFieldSizeY = null;
    [SerializeField] TextMeshProUGUI _inputFieldSizeYPlaceholder = null;
    [SerializeField] Button _buttonStartGame = null;
    [SerializeField] Button _buttonRestartGame = null;
    [SerializeField] Button _buttonEndTurn = null;

    [SerializeField] GridController _gridController = null;
    [SerializeField] TurnController _turnController = null;
    [SerializeField] DeckController _deckController = null;

    [SerializeField] TextMeshProUGUI _currentTurnText = null;
    [SerializeField] TextMeshProUGUI _playerOnTurnText = null;

    [SerializeField] Button _hideScoreboardButton = null;
    [SerializeField] GameObject _scoreboard = null;
    [SerializeField] TextMeshProUGUI _scoreBoardText = null;

    [SerializeField] Toggle _muteToggle = null;
     
    public void Initialize()
    {
        _inputFieldSizeXPlaceholder.text = GameConfigScriptableObject.Instance.DefaultX.ToString();
        _inputFieldSizeYPlaceholder.text = GameConfigScriptableObject.Instance.DefaultY.ToString();

        _buttonStartGame.onClick.AddListener(OnStartGame);
        _buttonRestartGame.onClick.AddListener(OnRestartGame);
        _buttonEndTurn.onClick.AddListener(OnEndTurn);
        _hideScoreboardButton.onClick.AddListener(() => _scoreboard.SetActive(false));

        _turnController.OnEndGame = OnEndGame;
        _gridController.OnSimulationOver += SetTurnText;

        _muteToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) SoundController.Instance.Mute();
            else SoundController.Instance.UnMute();
        });
    }

    private void OnStartGame()
    {
        InitializeGrid();
        _turnController.StartGame();
        SetTurnText();

        _buttonStartGame.gameObject.SetActive(false);
        _buttonRestartGame.gameObject.SetActive(true);

        _buttonEndTurn.gameObject.SetActive(true);
        _currentTurnText.gameObject.SetActive(true);
        _playerOnTurnText.gameObject.SetActive(true);
    }

    private void OnRestartGame()
    {
        _scoreboard.SetActive(false);

        _buttonEndTurn.gameObject.SetActive(true);
        _currentTurnText.gameObject.SetActive(true);
        _playerOnTurnText.gameObject.SetActive(true);

        _buttonStartGame.gameObject.SetActive(false);
        _buttonRestartGame.gameObject.SetActive(true);

        InitializeGrid();
        _gridController.SetSimulation();
        _turnController.ResetGame();
        _deckController.Initialize();
    }

    private void OnEndTurn()
    {
        _turnController.ChangeTurn();
        SetTurnText();
    }

    private void InitializeGrid()
    {
        int sizeX = GameConfigScriptableObject.Instance.DefaultX;
        int sizeY = GameConfigScriptableObject.Instance.DefaultY;

        if (!_inputFieldSizeX.text.Equals("") && !_inputFieldSizeY.text.Equals(""))
        {
            sizeX = int.Parse(_inputFieldSizeX.text);
            sizeY = int.Parse(_inputFieldSizeY.text);
        }

        if (!_gridController.GridInitialized) _gridController.InitializeGrid(sizeX, sizeY);
        else if (!_gridController.ResizeGrid(sizeX, sizeY)) _gridController.ClearGrid();
    }

    private void OnEndGame()
    {
        string scoreboardText = "";

        foreach(KeyValuePair<int,int> handPoints in _turnController.GetScores())
        {
            scoreboardText += $"Player {handPoints.Key + 1} : {handPoints.Value}\n";
        }

        _scoreBoardText.text = scoreboardText;
        _scoreboard.SetActive(true);

        _buttonEndTurn.gameObject.SetActive(false);
        _currentTurnText.gameObject.SetActive(false);
        _playerOnTurnText.gameObject.SetActive(false);
    }

    private void SetTurnText()
    {
        _currentTurnText.text = (_turnController.CurrentTurn + 1).ToString();
        _playerOnTurnText.gameObject.SetActive(_turnController.HandOnTurnId < CardGameScriptableObject.Instance.PlayerCount);
        _playerOnTurnText.text = (_turnController.HandOnTurnId + 1).ToString();
    }
}
