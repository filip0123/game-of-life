using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "PredefinedShape", menuName = "ScriptableObjects/PredefinedShapes", order = 2)]
public class PredefinedShapeScriptableObject : ScriptableObject
{
    private const string FileName = "PredefinedShape";
    private static PredefinedShapeScriptableObject _instance = null;
    public static PredefinedShapeScriptableObject Instance
    {
        get
        {
            if (_instance == null) _instance = Resources.Load(FileName) as PredefinedShapeScriptableObject;
            return _instance;
        }
    }

    [SerializeField] private CardView _cardViewPrefab = null;
    [SerializeField] private ShapeModel[] _shapes = null;

    public CardView CardViewPrefab => _cardViewPrefab;

    public ShapeModel GetRandomModelOfType(ShapeType type)
    {
        ShapeModel[] shapeTypeModels = _shapes.Where(x => x.ShapeType == type).ToArray();
        return shapeTypeModels.ElementAt(Random.Range(0, shapeTypeModels.Length));
    }

    public ShapeModel GetRandomModel()
    {
        return _shapes.ElementAt(Random.Range(0, _shapes.Length));
    }
}
