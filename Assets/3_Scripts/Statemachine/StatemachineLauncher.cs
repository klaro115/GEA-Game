using UnityEngine;
using System.Collections;

namespace Game
{
	[AddComponentMenu("Scripts/State Machine/Launcher")]
	public class StatemachineLauncher : MonoBehaviour
	{
		#region Fields

		public Gamestate startGameState = Gamestate.Ingame;

		#endregion
		#region Methods

		void Start ()
		{
			Statemachine.initialize(startGameState);
		}
		
		void Update ()
		{
			Statemachine.update();
		}

		#endregion
	}
}
