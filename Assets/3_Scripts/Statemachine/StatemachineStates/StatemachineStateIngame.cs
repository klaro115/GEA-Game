using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public class StatemachineStateIngame : StatemachineState
	{
		#region Fields
		
		private IngameState state = IngameState.None;	// Ingame game state.

		private Player player = null;					// Reference to the player character in scene.
		private List<Enemy> enemies = new List<Enemy>();// List of all enemies currently in scene.

		// TODO: implement UI elements 
		//private UiIngame uiIngame = null;

		//private static readonly string uiIngamePrefabName = "UiIngame";

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

		#endregion
		#region Methods
		
		public override bool initialize()
		{
			// Find existing player and enemy instances in scene:
			player = GameObject.FindObjectOfType<Player>();
			if(enemies == null) enemies = new List<Enemy>();
			Enemy[] newEnemies = GameObject.FindObjectsOfType<Enemy>();
			if(newEnemies != null)
				enemies.AddRange(newEnemies);

			// TODO:
			// - Spawn player if not present already.
			// - Find and/or spawn UI elements if not present already.

			// TODO:
			// - Spawn ingame UI group if not present already.
			// - Initialize ingame UI:
			/*
			uiIngame = GameObject.FindObjectOfType<UiIngame>();
			if(uiIngame == null)
			{
				UiIngame uiIngamePrefab = Resources.Load<UiIngame>("uiIngamePrefabName");
				GameObject uiIngameGO = Instantiate(uiIngamePrefab.gameObject, uiCanvas) as GameObject;
				uiIngame = uiIngameGO.GetComponent<UiIngame>();
			}
			uiIngame.initialize();
			*/

			setState(IngameState.Ingame);

			return true;
		}
		public override void shutdown ()
		{
			// Destroy player ship:
			GameObject.Destroy(player.gameObject);
			player = null;

			destroyAllEnemies();

			// TODO:
			// - Shut down ingame UI.
			// - Destroy ingame UI gameObject.
			/*
			if(uiIngame != null)
			{
				uiIngame.shutdown();
				MonoBehaviour.Destroy(uiIngame.gameObject);
			}
			*/

			// Reset ingame flag in main statemachine:
			Statemachine.IsIngame = false;

			// Get rid of any unused assets:
			Resources.UnloadUnusedAssets();
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
			//uiIngame.setState(state);

			return true;
		}

		private void destroyAllEnemies()
		{
			// Destroy remaining enemy characters in scene:
			foreach(Enemy enemy in enemies.ToArray())
			{
				GameObject.Destroy(enemy.gameObject);
			}
			// Clear enemies list:
			enemies.Clear();
		}
		
		#endregion
	}
}
