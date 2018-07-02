using UnityEngine;
using System.Collections;

namespace Game
{
	[System.Serializable]
	public abstract class StatemachineState
	{
		#region Fields

		//...

		#endregion
		#region Fields Static

		protected static readonly Gamestate[] allowedStatesStartup = new Gamestate[1] { Gamestate.MainMenu };
		protected static readonly Gamestate[] allowedStatesQuit = new Gamestate[1] { Gamestate.MainMenu };
		protected static readonly Gamestate[] allowedStatesMainMenu = new Gamestate[2] { Gamestate.Quit, Gamestate.Ingame };
		protected static readonly Gamestate[] allowedStatesIngameMenu = new Gamestate[2] { Gamestate.MainMenu, Gamestate.Ingame };

		#endregion
		#region Methods

		public abstract bool initialize();
		public abstract void shutdown();

		public virtual Gamestate[] getAllowedStates() { return allowedStatesStartup; }

		public virtual void update() { }

		#endregion
		#region Methods Static

		public static StatemachineState getSubStatemachine(Gamestate state)
		{
			StatemachineState instance = null;

			switch (state)
			{
			case Gamestate.MainMenu:
				instance = new StatemachineStateMainMenu();
				break;
			case Gamestate.Ingame:
				instance = new StatemachineStateIngame();
				break;
			case Gamestate.Quit:
				return null;
			// TODO: add instance creation for further states. (primarily ingame)
			default:
				Debug.LogError("StateMachineState: Error! Unknown game state: " + state.ToString());
				break;
			}

			//...

			return instance;
		}

		#endregion
	}
}
