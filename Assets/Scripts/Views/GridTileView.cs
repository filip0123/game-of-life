using UnityEngine;

public class GridTileView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _object = null;
    private int _positionX = 0;
    private int _positionY = 0;

    public Vector2Int Position => new Vector2Int(_positionX, _positionY);

    public void SetPosition(int x, int y)
    {
        _positionX = x;
        _positionY = y;
    }

    public void SetState(int state)
    {
        switch((TileState)state)
        {
            case TileState.empty:
                Hide();
                break;
            case TileState.live: 
                Show();
                break;
            case TileState.selected:
                Select();
                break;
            case TileState.playerOne:
                Show(0);
                break;
            case TileState.playerTwo:
                Show(1);
                break;
        }
    }

    private void Show(int playerId = -1)
    {
        _object.enabled = true;
        _object.material = playerId == -1 ? 
            GameConfigScriptableObject.Instance.MaterialLive : 
            CardGameScriptableObject.Instance.PlayerTileMaterials[playerId];
    }

    private void Hide()
    {
        _object.enabled = false;
    }

    private void Select()
    {
        _object.enabled = true;
        _object.material = GameConfigScriptableObject.Instance.MaterialSelected;
    }
}
