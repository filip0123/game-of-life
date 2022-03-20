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

    public void SetLive(bool isLive)
    {
        if (isLive) Show();
        else Hide();
    }

    private void Show()
    {
        _object.enabled = true;
    }

    private void Hide()
    {
        _object.enabled = false;
    }
}
