using System.Collections.Generic;
using UnityEngine;

public class CardDeck: SingletonObject<CardDeck>
{
    public List<CardSet> cardSets = new List<CardSet>();

    public CardSet GetCurrentCardSet()
    {
        CardSetType selectedType = GameManagerUX.Instance.selectedCardSetType;
        foreach (var cardSet in cardSets)
        {
            if (cardSet.setType == selectedType)
            {
                return cardSet;
            }
        }
        Debug.LogWarning($"선택된 CardSetType '{selectedType}'에 해당하는 CardSet이 없습니다.");
        return null;
    }
}