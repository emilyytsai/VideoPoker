using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	/// 
	/// The main game manager
	/// 
	public class GameManager : MonoBehaviour
	{
		//-//////////////////////////////////////////////////////////////////////
		/// 

		//script/class references
		public Deck deck;
		private UIManager UI_manager;
		public Player player;
        private HandEvaluator hand_evaluator;

		//list to store current hand; note* remember to pass 5 to deal_cards so player hand is 5
		public List<Card> player_hand = new List<Card>();

		//UI buttons
		public Button deal_button;
		public Button hold_button;
		public Button bet_button;

		//disolay the cards
        public List<Image> card_displays = new List<Image>();

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
			//initialize
            deck = new Deck();
			player = new Player();
            UI_manager = FindObjectOfType<UIManager>();
            hand_evaluator = new HandEvaluator();
            
            UI_manager.update_balance(player.balance);
            UI_manager.update_winnings(0);
		}
		
		//-//////////////////////////////////////////////////////////////////////
		/// 

		//new_round()
		//deal()
		//redeal()
		//update_card_displays()
		//evaluate_hand()

        //start a new round
        public void new_round()
        {
            //clear the current hand
            player_hand.Clear();
            
            //make sure deck is shuffled
            deck.shuffle_deck();
            
            //take away bet amt from players balance
			if (player.place_bet())
			{
				player.balance -= player.bet;
				UI_manager.update_balance(player.balance);
			}
        }
        
        //deal 5 cards
        public void deal()
        {
            player_hand = deck.deal_cards(5);
            update_card_displays();
        }
        
        //redeal the cards that werent held
        public void redeal(List<int> held_indexes)
        {
            //temp list for storing new hand
            List<Card> new_hand = new List<Card>();
            
            //go thru each card position
            for (int i = 0; i < 5; i++)
            {
                if (held_indexes.Contains(i))
                {
                    //keep held cards
                    new_hand.Add(player_hand[i]);
                }
                else
                {
                    //draw new card
                    Card new_card = deck.draw_card();
                    if (new_card != null)
                    {
                        new_hand.Add(new_card);
                    }
                }
            }
            
            //eeplace old hand w/ new one
            player_hand = new_hand;
            update_card_displays();
        }
        
        //update the card images display
        private void update_card_displays()
        {
            for (int i = 0; i < player_hand.Count; i++)
            {
                if (i < card_displays.Count)
                {
                    card_displays[i].sprite = player_hand[i].Card_sprite;
                }
            }
        }
        
        //eval current hand and return winnings
        public int evaluate_hand()
        {
            int winnings = hand_evaluator.evaluate_hand(player_hand, player.bet);
            
            //get hand type to display it
            string hand_type = hand_evaluator.hand_type(player_hand);
            
            //update ui with hand type if its a winning hand
            if (winnings > 0)
            {
                //use ui manager's method to display winning hand type
                UI_manager.display_winning_hand(hand_type);
            }
            
            return winnings;
        }
    }
}