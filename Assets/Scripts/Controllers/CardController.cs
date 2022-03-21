using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GridController _gridController = null;
    [SerializeField] private GameObject _cantPlaceCursor = null;

    private Camera _main = null;
    private CardView _selectedCardView = null;
    private bool _dragging = false;
    private bool _hoveringOverGrid = false;

    private GridTileView _selectedTile = null;

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

        if (_hoveringOverGrid) Place();
    }

    public void CardViewToGridShape()
    {
        _hoveringOverGrid = true;
        _selectedCardView.gameObject.SetActive(false);
    }

    public void GridShapeToCardView()
    {
        _hoveringOverGrid = false;
        _selectedCardView.gameObject.SetActive(true);
    }

    public void Place()
    {
        if (_gridController.CanSetShape(_selectedTile.Position, _selectedCardView.Shape.LogicalTileArrangement, _selectedCardView.Shape.SizeX, _selectedCardView.Shape.SizeY))
        {
            _gridController.SetShape(_selectedTile.Position, _selectedCardView.Shape.LogicalTileArrangement, _selectedCardView.Shape.SizeX, _selectedCardView.Shape.SizeY);
        }
    }

    private void Update()
    {
        if(_dragging)
        {
            Transform raycast = RaycastResolver.GetRaycastTransform(Layer.Tile);
            if(raycast != null && _selectedTile == null || raycast != _selectedTile.transform)
            {
                _selectedTile = raycast.GetComponent<GridTileView>();
            }

            _selectedCardView.transform.position = Input.mousePosition;
        }
    }
}
