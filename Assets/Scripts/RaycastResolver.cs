using UnityEngine;

public class RaycastResolver
{
    public static Transform _raycastHit = null;

    public static Transform Raycast(Layer layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << (int)layer;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask)) return hit.transform;

        return null;
    }
}
