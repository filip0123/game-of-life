using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class CardView : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI _nameText = null;
    [SerializeField] private Image _image = null;

    private ShapeModel _shape = null;
    public ShapeModel Shape => _shape;

    public Action OnPointerDownAction = null;

    public void InitializeView(ShapeModel shape)
    {
        _shape = shape; 
        _image.sprite = shape.CardImage;
        _nameText.text = shape.ShapeName;
    }

    public void OnPointerDown(PointerEventData data)
    {
        OnPointerDownAction?.Invoke();
    }
}
