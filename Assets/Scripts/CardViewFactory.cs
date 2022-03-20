using UnityEngine;

public class CardViewFactory : MonoBehaviour
{
    [SerializeField] private CardController _cardController = null;
    public CardView Create(ShapeModel shape, Transform parent)
    {
        CardView card = Instantiate(PredefinedShapeScriptableObject.Instance.CardViewPrefab, parent);
        card.InitializeView(shape,_cardController);
        return card;
    }
}
