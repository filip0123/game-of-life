using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{
    [SerializeField] CardViewFactory _factory = null;
    [SerializeField] Button _drawButton = null;
    [SerializeField] Transform _playerHandContainer = null;

    public void Initialize()
    {
        _drawButton.onClick.AddListener(() => _factory.Create(PredefinedShapeScriptableObject.Instance.GetRandomModel(), _playerHandContainer));
    }
}
