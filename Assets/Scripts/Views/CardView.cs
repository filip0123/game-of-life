using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI _nameText = null;
    [SerializeField] private Image _image = null;

    private CardController _cardController = null;
    private ShapeModel _shape = null;

    public ShapeModel Shape => _shape;

    public void InitializeView(ShapeModel shape, CardController controller)
    {
        _shape = shape; 
        _image.sprite = shape.CardImage;
        _nameText.text = shape.ShapeName;
        _cardController = controller;
    }

    public void OnPointerDown(PointerEventData data)
    {
        _cardController.StartDrag(this);
    }
}
