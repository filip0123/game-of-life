using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI _nameText = null;
    [SerializeField] private Image _image = null;

    private CardController _cardController = null;

    public void InitializeView(ShapeModel shape, CardController controller)
    {
        _image.sprite = shape.CardImage;
        _nameText.text = shape.ShapeName;
        _cardController = controller;
    }

    public void OnPointerDown(PointerEventData data)
    {
        _cardController.StartDrag(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _cardController.StopDragging();
    }
}
