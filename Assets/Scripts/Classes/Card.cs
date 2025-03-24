using UnityEngine;

public class Card
{
    //getters & setters
    //get value of the property -> allow to set/change that value (private only in card class)
    public string Suit { get; private set; }
    public string Rank { get; private set; }
    public Sprite Card_sprite { get; private set; }

    //constructor
    //initialize the card w/ its suit rank and sprite
    public Card(string suit, string rank, Sprite card_sprite)
    {
        Suit = suit;
        Rank = rank;
        Card_sprite = card_sprite;
    }

    //accessor/getter method
    //get the card names
    public string GetCardName()
    {
        return Rank + " of " + Suit;
        //would return smth like "Queen of Hearts"
    }
}
