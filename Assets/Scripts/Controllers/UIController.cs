using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputFieldSizeX = null;
    [SerializeField] TextMeshProUGUI _inputFieldSizeXPlaceholder = null;
    [SerializeField] TMP_InputField _inputFieldSizeY = null;
    [SerializeField] TextMeshProUGUI _inputFieldSizeYPlaceholder = null;
    [SerializeField] Button _buttonInitializeField = null;
    [SerializeField] Button _buttonStartSimulation = null;
    [SerializeField] Button _buttonRestartSimulation = null;
    [SerializeField] GridController _gridController; 
    
    private void Awake()
    {
        _gridController.Initialize();
        _buttonInitializeField.onClick.AddListener(OnInitializeField);
        _buttonStartSimulation.onClick.AddListener(OnStartSimulation);
        _buttonRestartSimulation.onClick.AddListener(SetSimulation);
        _inputFieldSizeXPlaceholder.text = GameConfigScriptableObject.Instance.DefaultX.ToString();
        _inputFieldSizeYPlaceholder.text = GameConfigScriptableObject.Instance.DefaultY.ToString();

        _inputFieldSizeX.onEndEdit.AddListener((x) => _buttonInitializeField.interactable = true);
        _inputFieldSizeY.onEndEdit.AddListener((x) => _buttonInitializeField.interactable = true);

        _buttonStartSimulation.gameObject.SetActive(false);
        _buttonRestartSimulation.gameObject.SetActive(false);
    }

    private void OnInitializeField()
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

        SetSimulation();

        _buttonInitializeField.interactable = false;
    }
    private void OnStartSimulation()
    {
        _gridController.StartSimulation();
        _buttonStartSimulation.gameObject.SetActive(false);
        _buttonRestartSimulation.gameObject.SetActive(true);
    }

    private void SetSimulation()
    {
        _gridController.SetSimulation();
        _buttonStartSimulation.gameObject.SetActive(true);
        _buttonRestartSimulation.gameObject.SetActive(false);
    }
}
