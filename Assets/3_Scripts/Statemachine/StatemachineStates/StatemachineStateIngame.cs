using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public class StatemachineStateIngame : StatemachineState
	{
		#region Types
		
		public enum State
		{
			None,

			Ingame,			// Player is actually playing the game.
			Paused,			// Ingame menu.
			NextLevel,		// Passage between levels, upgrades, score, etc.
			GameOver,		// Player has lost the game.
		}
		
		#endregion
		#region Fields
		
		private State state = State.None;

		private Player player = null;
		private List<Enemy> enemies = new List<Enemy>();

		#endregion
		#region Properties

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
			// Spawn player if not present already.
			// Find and/or spawn UI elements if not present already.

			setState(State.Ingame);

			return true;
		}
		public override void shutdown ()
		{
			// Destroy player ship:
			GameObject.Destroy(player.gameObject);
			player = null;

			// Destroy remaining enemy ships:
			foreach(Enemy enemy in enemies.ToArray())
			{
				GameObject.Destroy(enemy.gameObject);
			}
			enemies.Clear();

			// Get rid of any unused assets:
			Resources.UnloadUnusedAssets();
		}
		
		public override Gamestate[] getAllowedStates ()
		{
			return allowedStatesIngameMenu;
		}
		
		public bool setState(State newState)
		{
			if(newState == state)
			{
				Debug.LogError("StateMachineMainMenu: Error! State '" + newState.ToString() + "' already active!");
				return false;
			}

			// Set new sub statmachine state and set ingame flag:
			Statemachine.IsIngame = newState == State.Ingame;
			state = newState;

			//...

			return true;
		}
		
		#endregion
	}
}
