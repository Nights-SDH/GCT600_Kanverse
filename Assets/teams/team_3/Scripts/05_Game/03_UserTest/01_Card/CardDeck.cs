using System.Collections.Generic;
using UnityEngine;

public class CardDeck: SingletonObject<CardDeck>
{
    public List<NetworkCard> cards = new List<NetworkCard>();
}
