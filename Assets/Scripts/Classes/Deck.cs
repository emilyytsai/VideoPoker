using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    //card decks - list of card objects; card object/property from card class
    //only modify shuffled_deck list in this class
    public List<Card> cards { get; private set; }
    private List<Card> shuffled_deck;

    //suit & rank arrays
    private string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
    private string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" }; // Card ranks

    //constructor
    public Deck()
    {
        cards = new List<Card>();
        shuffled_deck = new List<Card>();
        initialize_deck();
        shuffle_deck();
    }

    //initialize deck w/ all 52 cards
    //note** the sprites have this naming pattern: img_card_c1.png
    //sprite names must match for Resources.Load<Sprite>() to work prorperly
    private void initialize_deck()
    {
        //map the suits and ranks to match filenames
        Dictionary<string, string> suit_map = new Dictionary<string, string>()
        {
            { "Clubs", "c" },
            { "Diamonds", "d" },
            { "Hearts", "h" },
            { "Spades", "s" }
        };

        Dictionary<string, int> rank_map = new Dictionary<string, int>()
        {
            { "Ace", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "10", 10 },
            { "Jack", 11 },
            { "Queen", 12 },
            { "King", 13 }
        };

        foreach (var suit in suit_map.Keys)
        {
            foreach (var rank in rank_map.Keys)
            {
                string sprite_name = $"im_card_{suit_map[suit]}{rank_map[rank]}"; //-> img_card_c1.png
                Sprite card_sprite = Resources.Load<Sprite>($"Resources/Art/Cards/{sprite_name}");

                Debug.Log($"Loaded: {sprite_name}");
                
                //create + add the card to deck
                cards.Add(new Card(suit, rank, card_sprite));
            }
        }
    }

    //shuffle the deck using Fisher-Yates algorithm**
    //resource: https://www.geeksforgeeks.org/shuffle-a-given-array-using-fisher-yates-shuffle-algorithm/
    public void shuffle_deck()
    {
        shuffled_deck.Clear();
        shuffled_deck.AddRange(cards);

        for (int i = shuffled_deck.Count - 1; i > 0; i--)
        {
            //pick a random index from 0 to i
            int j = Random.Range(0, i + 1);

            //swap i w/ j which is the rand index
            var temp = shuffled_deck[i];
            shuffled_deck[i] = shuffled_deck[j];
            shuffled_deck[j] = temp;
        }
    }

    //for dealing 5 cards
    public List<Card> deal_cards(int count)
    {
        List<Card> dealt_cards = new List<Card>();

        for (int i = 0; i < count; i++)
        {
            if (shuffled_deck.Count > 0)
            {
                dealt_cards.Add(shuffled_deck[0]);
                shuffled_deck.RemoveAt(0);
            }
        }

        return dealt_cards;
    }

    //draw new card from the shuffled deck; this is for re-dealing after hold
    public Card draw_card()
    {
        if (shuffled_deck.Count > 0)
        {
            var drawn_card = shuffled_deck[0];
            shuffled_deck.RemoveAt(0);
            return drawn_card;
        }
        return null;
    }
}
