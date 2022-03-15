using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputFieldSizeX = null;
    [SerializeField] TMP_InputField _inputFieldSizeY = null;
    [SerializeField] Button _buttonStartSimulation = null;
    [SerializeField] Button _buttonResetField = null;
    [SerializeField] GridView _gridView; 
    
    private void Initialize()
    {
    }
}
