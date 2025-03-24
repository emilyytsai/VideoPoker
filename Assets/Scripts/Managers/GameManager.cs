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
		private Player player;
        private HandEvaluator hand_evaluator;

		//list to store current hand; note* remember to pass 5 to deal_cards so player hand is 5
		public List<Card> player_hand = new List<Card>();

		//UI buttons
		public Button deal_button;
		public Button hold_button;
		public Button bet_button;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
		}
		
		//-//////////////////////////////////////////////////////////////////////
		/// 

		//new_round()
		//end_round
		//deal()

		//enable_holding()
		//from ui manager -> enable_hold_selection()
	}
}