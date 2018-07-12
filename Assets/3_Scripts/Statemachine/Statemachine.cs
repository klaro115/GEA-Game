using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
	public static class Statemachine
	{
		#region Fields

		private static Gamestate gameState = Gamestate.None;
		private static StatemachineState currentState = null;
		private static bool isIngame = false;

		#endregion
		#region Properties

		public static Gamestate GameState
		{
			get { return gameState; }
		}
		public static StatemachineState CurrentState
		{
			get { return currentState; }
		}
		public static bool IsIngame
		{
			set { isIngame = value; }
			get { return isIngame; }
		}

		#endregion
		#region Methods

		/// <summary>
		/// Initialize the statemachine, load settings, etc. Done exactly once on program start.
		/// </summary>
		public static bool initialize(Gamestate startGameState = Gamestate.Ingame)
		{
			gameState = Gamestate.None;

			//...

			// Initialize global static members:
			SoundHandler.initialize();

			// Load the first game state:
			return setState(startGameState);
			// NOTE: Set starting game state to 'ingame' or other values during in-engine testing...
		}
		/// <summary>
		/// Shuts down the statemachie and all dependencies. Unloads any active scene elements and states.
		/// </summary>
		public static void shutdown()
		{
			// Make sure the current game state is terminated properly:
			if(currentState != null)
			{
				currentState.shutdown();
				currentState = null;
			}

			// Terminate global static members:
			SoundHandler.shutdown();

			//...
		}

		public static bool setState(Gamestate newState)
		{
			if(gameState == newState)
			{
				Debug.LogError("Statemachine: GameState '" + newState.ToString() + "' already active!");
				return false;
			}

			// Verify if new state can be set from current state.
			if(currentState != null)
			{
				Gamestate[] allowedStates = currentState.getAllowedStates();
				if(allowedStates != null && !allowedStates.Contains(newState))
				{
					Debug.LogError("Statemachine: GameState '" + newState.ToString() +
						"' is not accessible from current state '" + gameState.ToString() + "'!");
					return false;
				}

				// Shutdown and drop previous state instance:
				currentState.shutdown();
				currentState = null;
			}

			// Apply new gameState:
			gameState = newState;

			// Instantiate and initialize new statemachien state instance:
			currentState = StatemachineState.getSubStatemachine(gameState);
			if(currentState != null)
			{
				currentState.initialize();
			}

			switch (gameState)
			{
			case Gamestate.Quit:
				// End game upon receiving the Quit state:
				Application.Quit();
				break;
			default:
				break;
			}

			// Return success:
			return true;
		}

		/// <summary>
		/// Continuously Updates any active game states.
		/// Call this via a monobehaviour's update loop or a coroutine.
		/// </summary>
		public static void update()
		{
			if(currentState == null) return;
			currentState.update();
		}

		#endregion
	}
}
