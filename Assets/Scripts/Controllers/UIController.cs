using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputFieldSizeX = null;
    [SerializeField] TextMeshProUGUI _inputFieldSizeXPlaceholder = null;
    [SerializeField] TMP_InputField _inputFieldSizeY = null;
    [SerializeField] TextMeshProUGUI _inputFieldSizeYPlaceholder = null;
    [SerializeField] Button _buttonStartGame = null;
    [SerializeField] Button _buttonRestartGame = null;
    [SerializeField] Button _buttonEndTurn = null;
    [SerializeField] GridController _gridController;
    [SerializeField] TurnController _turnController;
    
    public void Initialize()
    {
        _inputFieldSizeXPlaceholder.text = GameConfigScriptableObject.Instance.DefaultX.ToString();
        _inputFieldSizeYPlaceholder.text = GameConfigScriptableObject.Instance.DefaultY.ToString();

        _buttonStartGame.onClick.AddListener(OnStartGame);
        _buttonRestartGame.onClick.AddListener(OnRestartGame);
        _buttonEndTurn.onClick.AddListener(OnEndTurn);
    }

    private void OnStartGame()
    {
        InitializeGrid();
        _turnController.StartGame();

        _buttonStartGame.gameObject.SetActive(false);
        _buttonRestartGame.gameObject.SetActive(true);
    }

    private void OnRestartGame()
    {
        _buttonStartGame.gameObject.SetActive(false);
        _buttonRestartGame.gameObject.SetActive(true);
        _gridController.ClearGrid();
    }

    private void OnEndTurn()
    {
        _turnController.ChangeTurn();
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
        else _gridController.ResizeGrid(sizeX, sizeY);
    }
}
