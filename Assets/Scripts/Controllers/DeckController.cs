using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [SerializeField] CardViewFactory _factory = null;
    private List<List<int>> _cardLists = null;

    public void Initialize()
    {
        if (CardGameScriptableObject.Instance.PremadeDeck)
        {
            _cardLists = new List<List<int>>();
            for(int i = 0; i < CardGameScriptableObject.Instance.PlayerCount; ++i)
            {
                _cardLists.Add(new List<int>(CardGameScriptableObject.Instance.Decks[i].CardList));
            }
        }
    }

    public void Draw(HandController handController, int handId)
    {
        if (CardGameScriptableObject.Instance.PremadeDeck)
        {
            if (_cardLists[handId].Count > 0)
            {
                int drawIndex = Random.Range(0, _cardLists[handId].Count);
                int nextDrawId = _cardLists[handId].ElementAt(drawIndex);
                _factory.Create(PredefinedShapeScriptableObject.Instance.GetModelById(nextDrawId), handController);
                _cardLists[handId].RemoveAt(drawIndex);
            }
        }
        else
        { 
            _factory.Create(PredefinedShapeScriptableObject.Instance.GetRandomModel(), handController);
        }
    }
}
