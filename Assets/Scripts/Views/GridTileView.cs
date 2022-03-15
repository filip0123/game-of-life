using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileView : MonoBehaviour
{
    [SerializeField] MeshRenderer _object = null;

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
