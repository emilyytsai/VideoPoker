using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEvaluator
{
    //multipliers for different hand types
    private const int royal_flush_multiplier = 800;
    private const int straight_flush_multiplier = 50;
    private const int four_of_a_kind_multiplier = 25;
    private const int full_house_multiplier = 9;
    private const int flush_multiplier = 6;
    private const int straight_multiplier = 4;
    private const int three_of_a_kind_multiplier = 3;
    private const int two_pair_multiplier = 2;
    private const int jacks_or_better_multiplier = 1;

    //main evaluation method -> determine hand type and returns winnings
    public int evaluate_hand(List<Card> hand, int bet_amount)
    {
        //sort hand by rank
        List<Card> sorted_hand = new List<Card>(hand);
        //a and b are two cards being compared
        sorted_hand.Sort((a, b) => card_value(a.Rank).CompareTo(card_value(b.Rank)));
        
        //check for different hand types in descending order of value
        if (royal_flush(sorted_hand))
            return bet_amount * royal_flush_multiplier;
        
        if (straight_flush(sorted_hand))
            return bet_amount * straight_flush_multiplier;
        
        if (four_of_a_kind(sorted_hand))
            return bet_amount * four_of_a_kind_multiplier;
        
        if (full_house(sorted_hand))
            return bet_amount * full_house_multiplier;
        
        if (flush(sorted_hand))
            return bet_amount * flush_multiplier;
        
        if (straight(sorted_hand))
            return bet_amount * straight_multiplier;
        
        if (three_of_a_kind(sorted_hand))
            return bet_amount * three_of_a_kind_multiplier;
        
        if (two_pair(sorted_hand))
            return bet_amount * two_pair_multiplier;
        
        if (jacks_or_better(sorted_hand))
            return bet_amount * jacks_or_better_multiplier;
        
        //if not jacks or better -> no winning hand, return 0
        return 0;
    }

    //get the winning hand type as a string for display (in ui manager)
    public string hand_type(List<Card> hand)
    {

        //sort hand by rank again
        List<Card> sorted_hand = new List<Card>(hand);
        sorted_hand.Sort((a, b) => card_value(a.Rank).CompareTo(card_value(b.Rank)));
        
        if (royal_flush(sorted_hand))
            return "Royal Flush";
        
        if (straight_flush(sorted_hand))
            return "Straight Flush";
        
        if (four_of_a_kind(sorted_hand))
            return "Four of a Kind";
        
        if (full_house(sorted_hand))
            return "Full House";
        
        if (flush(sorted_hand))
            return "Flush";
        
        if (straight(sorted_hand))
            return "Straight";
        
        if (three_of_a_kind(sorted_hand))
            return "Three of a Kind";
        
        if (two_pair(sorted_hand))
            return "Two Pair";
        
        if (jacks_or_better(sorted_hand))
            return "Jacks or Better";
        
        return "No Winning Hand :(";
    }

    //convert card rank to numeric value -> use these numeric card values to compare cards
    private int card_value(string rank)
    {
        switch (rank)
        {
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "10": return 10;
            case "Jack": return 11;
            case "Queen": return 12;
            case "King": return 13;
            case "Ace": return 14;
            default: return 0;
        }
    }

    //hand type evaluation methods
    private bool royal_flush(List<Card> hand)
    {
        //royal flush = straight flush with ace and high card
        return straight_flush(hand) && 
            hand[4].Rank == "Ace" && 
            hand[0].Rank == "10";
    }
    
    private bool straight_flush(List<Card> hand)
    {
        //straight flush = both straight and a flush
        return flush(hand) && straight(hand);
    }
    
    private bool four_of_a_kind(List<Card> hand)
    {
        //group the cards by rank
        Dictionary<string, int> rank_count = count_ranks(hand);
        
        //check if any rank appears 4 times
        foreach (var count in rank_count.Values)
        {
            if (count == 4)
                return true;
        }
        
        return false;
    }
    
    private bool full_house(List<Card> hand)
    {
        Dictionary<string, int> rank_count = count_ranks(hand);
        
        bool three = false; //three of a kund
        bool pair = false;
        
        //check for three of a kind and a pair
        foreach (var count in rank_count.Values)
        {
            if (count == 3)
                three = true;
            else if (count == 2)
                pair = true;
        }
        
        return three && pair;
    }
    
    private bool flush(List<Card> hand)
    {
        //all cards have the same suit
        string suit = hand[0].Suit;
        
        foreach (var card in hand)
        {
            if (card.Suit != suit)
                return false;
        }
        
        return true;
    }
    
    private bool straight(List<Card> hand)
    {
        //check for 5 cards in sequence/order
        for (int i = 0; i < hand.Count - 1; i++)
        {
            int current_value = card_value(hand[i].Rank);
            int next_value = card_value(hand[i + 1].Rank);
            
            if (next_value - current_value != 1)
            {
                //check for special case: A2345
                if (i == 0 && hand[0].Rank == "2" && hand[4].Rank == "Ace")
                {
                    //or if its 2345A
                    if (hand[1].Rank == "3" && hand[2].Rank == "4" && hand[3].Rank == "5")
                        return true;
                }
                return false;
            }
        }
        
        return true;
    }
    
    private bool three_of_a_kind(List<Card> hand)
    {
        Dictionary<string, int> rank_count = count_ranks(hand);
        
        // Check if any rank appears 3 times
        foreach (var count in rank_count.Values)
        {
            if (count == 3)
                return true;
        }
        
        return false;
    }
    
    private bool two_pair(List<Card> hand)
    {
        // Group cards by rank
        Dictionary<string, int> rank_count = count_ranks(hand);
        
        int pairCount = 0;
        
        // Count the number of pairs
        foreach (var count in rank_count.Values)
        {
            if (count == 2)
                pairCount++;
        }
        
        return pairCount == 2;
    }
    
    private bool jacks_or_better(List<Card> hand)
    {
        // Group cards by rank
        Dictionary<string, int> rank_count = count_ranks(hand);
        
        // Check for a pair of Jacks or better
        foreach (var entry in rank_count)
        {
            if (entry.Value == 2) // Found a pair
            {
                int rankValue = card_value(entry.Key);
                if (rankValue >= 11) // Jack or higher
                    return true;
            }
        }
        
        return false;
    }
    
    //group the cards by rank using this dictionary for two pair, three/four of a kind, and full house
    private Dictionary<string, int> count_ranks(List<Card> hand)
    {
        Dictionary<string, int> rank_count = new Dictionary<string, int>();
        
        foreach (var card in hand)
        {
            if (rank_count.ContainsKey(card.Rank))
                rank_count[card.Rank]++;
            else
                rank_count[card.Rank] = 1;
        }
        
        return rank_count;
    }
}
