using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	///
	/// Manages UI including button events and updates to text fields
	/// 
	public class UIManager : MonoBehaviour
	{
		//text fields
		[SerializeField]
		private Text currentBalanceText = null;
		[SerializeField]
		private Text winningText = null;

		//buttons
		[SerializeField]
		private Button betButton = null;
		[SerializeField]
		private Button holdButton = null;
		[SerializeField]
		private Button dealButton = null; //also acts as a draw button

		//toggles for holding cards
		[SerializeField]
		private List<Toggle> hold_toggles = null;

		//betting amounts
		private int current_bet = 10; //default bet amt
        private const int bet_increment = 10; //raise bet by incremnts of 10
        private const int max_bet = 50; //limit

		//reference player class
		private Player player;

		//track if round is over
		private bool round_over = false;
		//track if its the first deal -> round ends after second deal
		private bool first_deal = true;

		//reference to game manager script
		[SerializeField]
        private GameManager game_manager;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
			game_manager = FindObjectOfType<GameManager>();
			player = game_manager.player;

			betButton.onClick.AddListener(OnBetButtonPressed);
			holdButton.onClick.AddListener(OnHoldButtonPressed);
			dealButton.onClick.AddListener(OnDealButtonPressed);
			
			//cannot hold at start -> can only hold after dealing
			holdButton.interactable = false;

			//update UI text
            update_balance(player.balance);
            update_winnings(0);

			//cannot toggle at start
            foreach (var toggle in hold_toggles)
            {
                toggle.gameObject.SetActive(false);
            }
		}

		//-//////////////////////////////////////////////////////////////////////
		///
		/// Events that triggers when bet/deal button is pressed
		/// 
		private void OnBetButtonPressed()
		{
			//prevent betting after round ends
			if (round_over && !first_deal) return;
			//cant bet when balance is low
			if (player.balance < bet_increment) return;

            //increase bet when button pressed
            current_bet += bet_increment;
			//back to default bet amt if player tries to bet over 100
            if (current_bet > max_bet) current_bet = 10;
            player.bet = current_bet;

			//update the bet display text
            betButton.GetComponentInChildren<Text>().text = "BET: " + current_bet;
			Debug.Log("current bet updated to: " + current_bet);
        
		}

		//start a new round or re-deal
        private void OnDealButtonPressed()
        {
			//not enough money to bet
            if (player.balance < player.bet) return;

			//new round after redeal
			if (first_deal)
            {
				//first deal = start a new round
                round_over = false;
                first_deal = false;

                //reset message from the last round
                winningText.text = "";

				//NOTE** uncomment after game manager implemntation
				//deal cards & start round
				game_manager.new_round();
				game_manager.deal();

                dealButton.interactable = false;
                holdButton.interactable = true; //enable hold button for card selection
				betButton.interactable = false; //bet cannot be changed after dealing
				dealButton.GetComponentInChildren<Text>().text = "DRAW"; //change deal to draw
            }
			//implemenitng re-deal logic
			else
            {
                //second deal/re-deal) = end the round
                List<int> held_indexes = get_indexes();
                
                //redeal cards & evaluate the hand
                game_manager.redeal(held_indexes);
                int winnings = game_manager.evaluate_hand();
                
                //update players balance with winnings
                if (winnings > 0)
				{
                    player.add_winnings(winnings);
                    update_winnings(winnings);
                } 
				else 
				{
                    update_winnings(0);
                }
                update_balance(player.balance);
                
                //reset the UI for next round
                round_over = true;
                first_deal = true;
                dealButton.GetComponentInChildren<Text>().text = "DEAL";
                betButton.interactable = true;
                holdButton.interactable = false;
                
                //disable hold toggles and reset them
                foreach (var toggle in hold_toggles)
                {
                    toggle.isOn = false;
                    toggle.gameObject.SetActive(false);
                }
            }
        }


		//-//////////////////////////////////////////////////////////////////////
		///
		/// Events that triggers when hold button is pressed
		/// 
		private void OnHoldButtonPressed()
        {
            enable_hold_selection();
            holdButton.interactable = false;
			dealButton.interactable = true; //enable the draw/deal button for second deal
		}

		//card selection for players on which cards to hold
		public void enable_hold_selection()
        {
            foreach (var toggle in hold_toggles)
            {
                toggle.gameObject.SetActive(true); //enable hold toggles
				Debug.Log("Toggles = on");
            }
        }

		//get the indexes of the cards that are being held
        public List<int> get_indexes()
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < hold_toggles.Count; i++)
            {
                if (hold_toggles[i].isOn)
                {
					//store number of held cards
                    indexes.Add(i);
                }
            }
            return indexes;
        }

		//-//////////////////////////////////////////////////////////////////////


		public void update_balance(int balance)
        {
            currentBalanceText.text = "Balance: " + balance + " Credits";
        }

		//how much credits won per round
        public void update_winnings(int winnings)
        {
            if (winnings > 0)
			{
                winningText.text = "\nYou won: " + winnings + " Credits";
			}
            else
			{
                winningText.text = "";
			}
        }

		//display the hand type that won
        public void display_winning_hand(string hand_type)
        {
            if (!string.IsNullOrEmpty(hand_type))
            {
                winningText.text = hand_type + "! " + winningText.text;
				Debug.Log("displayed winnning hand");
            }
        }
	}
}