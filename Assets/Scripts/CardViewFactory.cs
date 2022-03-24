using UnityEngine;

public class CardViewFactory : MonoBehaviour
{
    public CardView Create(ShapeModel shape, HandController handController)
    {
        CardView card = Instantiate(PredefinedShapeScriptableObject.Instance.CardViewPrefab, handController.transform);
        card.InitializeView(shape);
        handController.AddCard(card);
        return card;
    }
}
