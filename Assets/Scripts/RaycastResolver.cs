using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastResolver
{
    public static Transform _raycastHit = null;

    private static void Raycast(Layer layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << (int)layer;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            if (_raycastHit == null || hit.transform != _raycastHit) _raycastHit = hit.transform;
        }
        else
        {
            _raycastHit = null;
        }
    }

    public static Transform GetRaycastTransform(Layer layer)
    {
        Raycast(layer);
        return _raycastHit;
    }
}
