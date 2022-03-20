using UnityEngine;

public class CardController : MonoBehaviour
{
    Camera _main = null;
    CardView _selectedCardView = null;
    bool _dragging = false;

    Vector2 _originalPosition = Vector2.zero;
    Camera Main
    {
        get 
        {
            if (_main == null) _main = Camera.main;
            return _main;
        }
    }

    public void StartDrag(CardView cardView)
    {
        _originalPosition = cardView.transform.position;
        _selectedCardView = cardView;
        _dragging = true;
    }

    public void StopDragging()
    {
        _selectedCardView.transform.position = _originalPosition;
        _selectedCardView = null;
        _dragging = false;
    }

    public void CardViewToGridShape()
    {

    }

    public void GridShapeToCardView()
    {

    }

    private void Update()
    {
        if(_dragging)
        {
            _selectedCardView.transform.position = Input.mousePosition;
        }
    }
}
