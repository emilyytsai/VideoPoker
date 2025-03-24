using UnityEngine;

public class Player
{

//for managing player balance, bet amount, and winnings

    //player starts off w/ 500 credit balence
    public int balance = 500;
    //default betting value
    public int bet = 10;

    //remove the bet amount from player's balance if they have sufficent amt of creds
    public bool place_bet()
    {
        if (balance >= bet)
        {
            balance -= bet;
            return true;
        }
        return false;
    }

    //method to add wining amt to the players balance
    public void add_winnings(int amount)
    {
        balance += amount;
    }

    //confirm instantiation
    public Player()
    {
        Debug.Log("player instantiated with balance: " + balance);
    }
}
