using UnityEngine;

public enum CardSetType
{
    Things,
    Sports,
    Transports,
    Office,
    Food
}

[System.Serializable]
public class CardSet
{
    public CardSetType setType;
    public Sprite[] cardSprites;
}