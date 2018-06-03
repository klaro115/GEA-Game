using UnityEngine;
using System.Collections;

namespace Game
{
	public static class EnemySpawner
	{
		#region Fields

		private static Level currentLevel = null;
		private static Wave currentWave = null;
		private static int currentLevelIndex = 0;
		private static int currentWaveIndex = 0;

		private static float nextWaveTime = 0.0f;

		#endregion
		#region Methods

		public static void initialize()
		{
			// Reset all values in order to start a new game session:
			currentLevel = null;
			currentWave = null;
			currentLevelIndex = 0;
			currentWaveIndex = 0;

			// Start spawning enemy waves right away after 1 second:
			nextWaveTime = Time.time + 1.0f;
		}

		public static void update()
		{
			if(nextWaveTime >= Time.time)
			{
				nextWaveTime = currentLevel.waveInterval;
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

			// Set level active and activate first wave:
			currentLevel = newLevel;
			currentWave = newLevel.waves[0];
			currentWaveIndex = 0;

			// Reset wave start timer:
			nextWaveTime = Time.time + 1.0f;

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
		}
		public static void notifyWaveCompleted()
		{
			if(currentLevel == null || currentWave == null)
			{
				Debug.LogError("EnemySpawner: Error! Wave completed even though no wave is currently active!");
				return;
			}

			// Go to the next wave:
			currentWaveIndex++;

			// Check if this was the last wave of the level:
			if(currentWaveIndex >= currentLevel.waves.Length)
			{
				notifyLevelCompleted();
			}
			// If more waves are yet to come:
			else
			{
				currentWave = currentLevel.waves[currentWaveIndex];
			}

			// TODO: Reset wave timer.
		}

		#endregion
	}
}
