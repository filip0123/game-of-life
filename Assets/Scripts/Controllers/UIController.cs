using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputFieldSizeX = null;
    [SerializeField] TMP_InputField _inputFieldSizeY = null;
    [SerializeField] Button _buttonStartSimulation = null;
    [SerializeField] Button _buttonResetField = null;
    [SerializeField] GridController _gridController; 
    
    private void Awake()
    {
        _gridController.Initialize();
        _buttonStartSimulation.onClick.AddListener(OnStartSimulation);
    }

    private void OnStartSimulation()
    {
        if(_gridController.GridInitialized)
        {
            _gridController.ResizeGrid(int.Parse(_inputFieldSizeX.text), int.Parse(_inputFieldSizeY.text));
        }
        else
        {
            _gridController.InitializeGrid(int.Parse(_inputFieldSizeX.text), int.Parse(_inputFieldSizeY.text));
        }

        _gridController.StartSimulation();
    }

}
