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

		public static StatemachineState getState(Gamestate state)
		{
			StatemachineState instance = null;

			switch (state)
			{
			case Gamestate.MainMenu:
				instance = new StatemachineStateMainMenu();
				break;
			// TODO: add instance creation for further states. (primarily ingame)
			default:
				break;
			}

			//...

			return instance;
		}

		#endregion
	}
}
