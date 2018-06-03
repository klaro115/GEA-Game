using UnityEngine;
using System.Collections;

namespace Game
{
	public static class EnemySpawner
	{
		#region Fields

		private static Level currentLevel = null;
		private static Wave currentWave = null;
		//private static int currentLevelIndex = 0;

		private static float nextWaveTime = 0.0f;
		private static float nextSpawnTime = 0.0f;

		#endregion
		#region Methods

		public static void initialize()
		{
			// Reset all values in order to start a new game session:
			currentLevel = null;
			currentWave = null;
			//currentLevelIndex = 0;

			// Start spawning enemy waves right away after 1 second:
			resetWaveTimer();
		}

		public static void update()
		{
			// Start next wave if the time has come and the previous wave has rolled out completely:
			if(currentWave.currentSpawnIndex >= currentWave.enemies.Length &&  nextWaveTime >= Time.time)
			{
				nextWaveTime = Time.time + currentLevel.waveInterval;
				notifyWaveCompleted();
			}

			// Spawn the current wave's enemies one after the other, possibly with a reccuring delay:
			if(Time.time >= nextSpawnTime)
			{
				nextSpawnTime = Time.time + currentWave.spawnInterval;
				spawnNextEnemy();
			}
		}

		/// <summary>
		/// Load a level resource/asset from HDD:
		/// </summary>
		/// <returns><c>true</c>, if level asset was loaded and set active, <c>false</c> otherwise.</returns>
		/// <param name="newLevelName">New level asset name.</param>
		public static bool loadLevel(string newLevelName)
		{
			if(newLevelName == null || newLevelName.CompareTo("") == 0)
			{
				Debug.LogError("EnemySpawner: Error! ");
				return false;
			}

			Level newLevel = Resources.Load<Level>(newLevelName);
			return setLevel(newLevel);
		}
		/// <summary>
		/// Sets a new level active.
		/// </summary>
		/// <returns><c>true</c>, if the level was set active, <c>false</c> otherwise.</returns>
		/// <param name="levelPrefab">New level asset.</param>
		public static bool setLevel(Level levelPrefab)
		{
			if(levelPrefab == null || levelPrefab.waves == null)
			{
				Debug.LogError("EnemySpawner: Error! Cannot set null or empty level active!");
				return false;
			}

			// Instantiate level from prefab:
			Level newLevel = MonoBehaviour.Instantiate(levelPrefab);

			Debug.Log("TEST: Current level set to: " + newLevel.name);

			// Set level active and activate first wave:
			currentLevel = newLevel;
			currentWave = newLevel.waves[0];
			currentLevel.currentWaveIndex = 0;
			currentWave.currentSpawnIndex = 0;

			// Reset wave start timer:
			resetWaveTimer();

			return true;
		}

		public static void notifyLevelCompleted()
		{
			if(currentLevel == null)
			{
				Debug.LogError("EnemySpawner: Error! Level completed even though no level is currently active!");
				return;
			}

			// TODO: Add level completion bonus points.
			// TODO: Switch ingame sub-state to gameOver or whatever.
			// TODO: Select, load and start next level when done (doesn't need to happen here).

			setLevel(currentLevel);	// TODO: Fetch/Load next level, then assign here instead of 'currentLevel'!
		}
		public static void notifyWaveCompleted()
		{
			if(currentLevel == null || currentWave == null)
			{
				Debug.LogError("EnemySpawner: Error! Wave completed even though no wave is currently active!");
				return;
			}

			// Go to the next wave:
			currentLevel.currentWaveIndex++;

			Debug.Log("TEST: Wave completed, starting wave: " + currentLevel.currentWaveIndex);

			// Check if this was the last wave of the level:
			if(currentLevel.currentWaveIndex >= currentLevel.waves.Length)
			{
				notifyLevelCompleted();
			}
			// If more waves are yet to come:
			else
			{
				currentWave = currentLevel.waves[currentLevel.currentWaveIndex];
				resetWaveTimer();
			}
		}

		private static void resetWaveTimer()
		{
			nextWaveTime = Time.time + 1.0f;
			nextSpawnTime = nextWaveTime;
		}

		private static void spawnNextEnemy()
		{
			Debug.Log("TEST: Spawning next enemy.");

			int spawnIndex = currentWave.currentSpawnIndex;
			if(spawnIndex < currentWave.enemies.Length)
			{
				// Fetch next enemy prefab, then spawn it in scene:
				Enemy enemyPrefab = currentWave.enemies[spawnIndex];
				GameObject enemyGO = MonoBehaviour.Instantiate(enemyPrefab.gameObject) as GameObject;
				Enemy enemy = enemyGO.GetComponent<Enemy>();

				Debug.Log("TEST: Spawned enemy: " + enemy.name);

				// Position enemy instance at its assigned starting position:
				enemy.transform.position = enemy.flightBehaviour[0];
				// TODO: Currently using world space coords, use screen-normalized coordinates instead!

				// Register newly spawned enemy with the ingame submachine:
				StatemachineStateIngame ingameState = Statemachine.CurrentState as StatemachineStateIngame;
				if(ingameState != null)
				{
					ingameState.Enemies.Add(enemy);
				}
			}
		}

		#endregion
	}
}
