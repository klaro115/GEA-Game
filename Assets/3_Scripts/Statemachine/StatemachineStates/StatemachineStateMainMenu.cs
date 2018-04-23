using UnityEngine;
using System.Collections;

namespace Game
{
	[System.Serializable]
	public class StatemachineStateMainMenu : StatemachineState
	{
		#region Fields

		//...

		#endregion
		#region Methods

		public override bool initialize()
		{
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

		#endregion
	}
}
