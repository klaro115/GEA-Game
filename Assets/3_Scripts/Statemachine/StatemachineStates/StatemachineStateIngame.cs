using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Game.UI;

namespace Game
{
	public class StatemachineStateIngame : StatemachineState
	{
		#region Fields
		
		private IngameState state = IngameState.None;	// Ingame game state.

		private Player player = null;					// Reference to the player character in scene.
		private List<Enemy> enemies = new List<Enemy>();// List of all enemies currently in scene.

		private int currentScore = 0;

		private UiIngame uiIngame = null;

		private static readonly string playerPrefabName = "Player";
		private static readonly string uiIngamePrefabName = "UiIngame";

		#endregion
		#region Properties

		public IngameState IngameState
		{
			get { return state; }
		}
		public Player Player
		{
			get { return player; }
		}
		public List<Enemy> Enemies
		{
			get { return enemies; }
		}
		public int Score
		{
			get { return currentScore; }
		}

		#endregion
		#region Methods
		
		public override bool initialize()
		{
			state = IngameState.None;

			// Find existing enemy instances in scene:
			if(enemies == null) enemies = new List<Enemy>();
			Enemy[] newEnemies = GameObject.FindObjectsOfType<Enemy>();
			if(newEnemies != null)
				enemies.AddRange(newEnemies);

			// Find or spawn player character in scene:
			player = GameObject.FindObjectOfType<Player>();
			if(player == null)
			{
				Player playerPrefab = Resources.Load<Player>(playerPrefabName);
				GameObject playerGO = MonoBehaviour.Instantiate(playerPrefab.gameObject) as GameObject;
				player = playerGO.GetComponent<Player>();
			}
			
			// Reset score to 0 points:
			currentScore = 0;

			// Spawn ingame UI group if not present already:
			uiIngame = GameObject.FindObjectOfType<UiIngame>();
			if(uiIngame == null)
			{
				RectTransform uiCanvas = GameObject.FindObjectOfType<Canvas>().transform as RectTransform;
				UiIngame uiIngamePrefab = Resources.Load<UiIngame>(uiIngamePrefabName);
				GameObject uiIngameGO = MonoBehaviour.Instantiate(uiIngamePrefab.gameObject, uiCanvas) as GameObject;
				uiIngame = uiIngameGO.GetComponent<UiIngame>();
			}
			// Initialize ingame UI:
			uiIngame.initialize();


			setState(IngameState.Ingame);

			return true;
		}
		public override void shutdown ()
		{
			// Destroy player ship:
			GameObject.Destroy(player.gameObject);
			player = null;

			destroyAllEnemies();

			if(uiIngame != null)
			{
				// Shut down ingame UI:
				uiIngame.shutdown();
				// Destroy ingame UI gameObject:
				MonoBehaviour.Destroy(uiIngame.gameObject);
			}

			// Reset ingame flag in main statemachine:
			Statemachine.IsIngame = false;

			// Get rid of any unused assets:
			Resources.UnloadUnusedAssets();
		}

		public void restartGame()
		{
			// Shutdown and terminate all ingame states, then restart everything from scratch:
			shutdown();
			initialize();
		}
		
		public override Gamestate[] getAllowedStates ()
		{
			return allowedStatesIngameMenu;
		}
		
		public bool setState(IngameState newState)
		{
			if(newState == state)
			{
				Debug.LogError("StateMachineMainMenu: Error! State '" + newState.ToString() + "' already active!");
				return false;
			}

			// Set new sub statmachine state and set ingame flag:
			Statemachine.IsIngame = newState == IngameState.Ingame;
			state = newState;

			switch (state)
			{
			case IngameState.NextLevel:
				// Destroy any remaining enemies:
				destroyAllEnemies();
				//...
				break;
			case IngameState.GameOver:
				// Destroy any remaining enemies:
				destroyAllEnemies();
				//...
				break;
			default:
				break;
			}

			//TODO: Tell ingame UI to switch to switch to the appropriate mode as well.
			uiIngame.setState(state);

			return true;
		}

		public void addScore(int points)
		{
			currentScore += points;
		}

		private void destroyAllEnemies()
		{
			// Destroy remaining enemy characters in scene:
			foreach(Enemy enemy in enemies.ToArray())
			{
				if(enemy != null) GameObject.Destroy(enemy.gameObject);
			}
			// Clear enemies list:
			enemies.Clear();
		}
		
		#endregion
		#region Methods Static

		/// <summary>
		/// Get the current ingame substatemachine:
		/// </summary>
		/// <returns>The ingame statemachine instance.</returns>
		public static StatemachineStateIngame getStatemachine()
		{
			if(Statemachine.GameState == Gamestate.Ingame)
			{
				return Statemachine.CurrentState as StatemachineStateIngame;
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
