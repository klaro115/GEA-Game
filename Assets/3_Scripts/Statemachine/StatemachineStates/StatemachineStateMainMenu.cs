using UnityEngine;
using System.Collections;

namespace Game
{
	[System.Serializable]
	public class StatemachineStateMainMenu : StatemachineState
	{
		#region Types

		public enum State
		{
			MainMenu,
			Credits,
			Settings
		}

		#endregion
		#region Fields

		private State state = State.MainMenu;

		//private UiMainMenuScreen uiMainMenu = null;

		#endregion
		#region Methods

		public override bool initialize()
		{
			//uiMainMenu = GameObject.FindObjectOfType<UiMainMenuScreen>();
			//uiMainMenu.initialize();

			//...

			return true;
		}
		public override void shutdown ()
		{
			//...
		}

		public override Gamestate[] getAllowedStates ()
		{
			return allowedStatesMainMenu;
		}

		public bool setState(State newState)
		{
			if(newState == state)
			{
				Debug.LogError("StateMachineMainMenu: Error! State '" + newState.ToString() + "' already active!");
				return false;
			}

			//...

			state = newState;

			return true;
		}

		#endregion
	}
}
