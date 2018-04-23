using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
	public static class Statemachine
	{
		#region Fields

		private static readonly Gamestate startGameState = Gamestate.Startup;
		private static Gamestate gameState = Gamestate.None;
		private static StatemachineState currentState = null;

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

		#endregion
		#region Methods

		/// <summary>
		/// Initialize the statemachine, load settings, etc. Done exactly once on program start.
		/// </summary>
		public static bool initialize()
		{
			gameState = Gamestate.None;

			//...

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
			}

			// Apply new gameState:
			gameState = newState;

			// Shutdown and drop previous state instance:
			if(currentState != null)
			{
				currentState.shutdown();
				currentState = null;
			}

			// Instantiate and initialize new statemachien state instance:
			currentState = StatemachineState.getState(gameState);
			if(currentState != null)
			{
				currentState.initialize();
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
			if(currentState != null)
			{
				currentState.update();
			}
		}

		#endregion
	}
}
