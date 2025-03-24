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
		[SerializeField]
		private Text currentBalanceText = null;

		[SerializeField]
		private Text winningText = null;

		[SerializeField]
		private Button betButton = null;
		private Button holdButton = null;
		private Button dealButton = null; //also acts as a draw button

		[SerializeField]
		//toggles for holding cards
		private List<Toggle> hold_toggles = null;

		//betting amounts
		private int current_bet = 10; //default bet amt
        private const int bet_increment = 10; //raise bet by incremnts of 10
        private const int max_bet = 100; //limit

		//reference player class
		private Player player;

		//track if round is over
		private bool round_over = false;
		//track if its the first deal -> round ends after second deal
		private bool first_deal = true;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
			betButton.onClick.AddListener(OnBetButtonPressed);
			holdButton.onClick.AddListener(OnHoldButtonPressed);
			dealButton.onClick.AddListener(OnDealButtonPressed);
			
            update_balance(player.balance);
            update_winnings(0);
		}

		//-//////////////////////////////////////////////////////////////////////
		///
		/// Events that triggers when bet/deal button is pressed
		/// 
		private void OnBetButtonPressed()
		{
			//cant bet when balance is low
			if (player.balance < bet_increment) return;

            //increase bet when button pressed
            current_bet += bet_increment;
			//back to default bet amt if player tries to bet over 100
            if (current_bet > max_bet) current_bet = 10;

            player.bet = current_bet;
		}

		//start a new round or re-deal
        private void OnDealButtonPressed()
        {
			//not enough money to bet
            if (player.balance < player.bet) return;

			//new round after redeal
			if (!round_over)
            {
                //reset message from the last round
                winningText.text = "";

				//NOTE** uncomment after game manager implemntation
				//deal cards & start round
				//FindObjectOfType<GameManager>().new_round();
				//FindObjectOfType<GameManager>().deal();

                dealButton.interactable = false;
                holdButton.interactable = true; // Enable hold button for card selection

                first_deal = false; // Set to false after first deal
            
            }

			//reset message from the last round
            winningText.text = "";
			//NOTE** uncomment after game manager implemntation
            //FindObjectOfType<GameManager>().new_round();
			//FindObjectOfType<GameManager>().deal();
        }


		//-//////////////////////////////////////////////////////////////////////
		///
		/// Events that triggers when hold button is pressed
		/// 
		private void OnHoldButtonPressed()
        {
            enable_hold_selection();
            holdButton.interactable = false;
		}

		//card selection for players on which cards to hold
		public void enable_hold_selection()
        {
            foreach (var toggle in hold_toggles)
            {
                toggle.gameObject.SetActive(true); // Show hold toggles
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
            currentBalanceText.text = "Balance: " + balance.ToString() + "Credits";
        }

		//how much credits won per round
        public void update_winnings(int winnings)
        {
            if (winnings > 0)
			{
                winningText.text = "Jacks or Better! You won: " + winnings.ToString() + "Credits";
			}
            else
			{
                winningText.text = "";
			}
        }
	}
}