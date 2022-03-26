using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GridController _gridController = null;
    [SerializeField] private GameObject _cantPlaceCursor = null;

    private CardView _selectedCardView = null;
    private bool _dragging = false;
    private bool _gridShapeForm = false;

    private GridTileView _selectedTile = null;

    private Vector2 _originalPosition = Vector2.zero;

    public System.Action OnCardPlaced = null;

    public void StartDrag(CardView cardView, HandController ownerHand)
    {
        _originalPosition = cardView.transform.position;
        _selectedCardView = cardView;
        _dragging = true;
    }

    public void StopDragging()
    {
        if (_gridShapeForm && CanPlace()) Place();
        else ResetCard();
    }

    private void CardViewToGridShape()
    {
        _gridShapeForm = true;
        _selectedCardView.gameObject.SetActive(false);
    }

    private void GridShapeToCardView()
    {
        _gridController.ClearPreviews();
        _gridShapeForm = false;
        _selectedCardView.gameObject.SetActive(true);
    }

    private void ResetCard()
    {
        _cantPlaceCursor.SetActive(false);
        _selectedCardView.transform.position = _originalPosition;
        _selectedCardView.gameObject.SetActive(true);
        _selectedCardView = null;
        _dragging = false;
        _gridShapeForm = false;
    }

    private void Place()
    {
        _gridController.SetShape(_selectedTile.Position, _selectedCardView.Shape.LogicalTileArrangement, _selectedCardView.Shape.SizeX, _selectedCardView.Shape.SizeY);

        OnCardPlaced?.Invoke();

        Destroy(_selectedCardView);
        _selectedCardView = null;
        _dragging = false;
        _gridShapeForm = false;
    }

    private void PreviewSelected()
    {
        _gridController.ClearPreviews();

        if (CanPlace())
        {
            _gridController.PreviewShape(_selectedTile.Position, _selectedCardView.Shape.LogicalTileArrangement, _selectedCardView.Shape.SizeX, _selectedCardView.Shape.SizeY);
        }
        else
        {
            _cantPlaceCursor.SetActive(true);
        }
    }

    private bool CanPlace()
    {
        bool canPlace = _gridController.CanSetShape(_selectedTile.Position, _selectedCardView.Shape.LogicalTileArrangement, _selectedCardView.Shape.SizeX, _selectedCardView.Shape.SizeY);

        _cantPlaceCursor.SetActive(!canPlace);
        if(!canPlace) _cantPlaceCursor.transform.position = Input.mousePosition;
        return canPlace;
    }

    private void Update()
    {
        if (_dragging)
        {
            Transform raycast = RaycastResolver.GetRaycastTransform(Layer.Tile);
            if (raycast != null && (_selectedTile == null || raycast != _selectedTile.transform))
            {
                if (!_gridShapeForm) CardViewToGridShape();

                _selectedTile = raycast.GetComponent<GridTileView>();
                PreviewSelected();
            }
            else if (raycast == null && _gridShapeForm)
            {
                GridShapeToCardView();
            }

            _selectedCardView.transform.position = Input.mousePosition;

            if (Input.GetMouseButtonUp(0)) StopDragging();
        }
    }
}
