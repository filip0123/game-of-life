using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [SerializeField] CardViewFactory _factory = null;
    private List<int> _cardList = null;

    public void Initialize()
    {
        if (CardGameScriptableObject.Instance.PremadeDeck)
        {
            _cardList = new List<int>(CardGameScriptableObject.Instance.Player1Deck);
        }
    }

    public void Draw(HandController handController)
    {
        if (CardGameScriptableObject.Instance.PremadeDeck)
        {
            if(_cardList.Count > 0)
            {
                int drawIndex = Random.Range(0, _cardList.Count);
                int nextDrawId = _cardList.ElementAt(drawIndex);
                _factory.Create(PredefinedShapeScriptableObject.Instance.GetModelById(nextDrawId), handController);
                _cardList.RemoveAt(drawIndex);
            }
        }
        else
        { 
            _factory.Create(PredefinedShapeScriptableObject.Instance.GetRandomModel(), handController);
        }
    }
}
