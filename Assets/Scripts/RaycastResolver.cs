using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastResolver : MonoBehaviour
{
    public static Transform _raycastHit = null;

    private static void Raycast(Layer layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, (int)layer))
        {
            if (_raycastHit == null && hit.transform != _raycastHit) _raycastHit = hit.transform;
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
