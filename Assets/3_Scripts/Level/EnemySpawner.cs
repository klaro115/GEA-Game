using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public static class EnemySpawner
	{
		#region Types

		private enum SpawnEventType
		{
			NextWave,
			NextLevel,
			SpawnEnemy,
		}
		private struct SpawnEvent
		{
			public SpawnEventType type;	// The type of event this represents.
			public Enemy enemyPrefab;	// Which enemy to spawn if this is a spawn event.
			public Wave nextWave;		// Which wave to start if this is a wave event.
			public float timestamp;
		}

		#endregion
		#region Fields

		private static Level currentLevel = null;
		private static Wave currentWave = null;
		//private static int currentLevelIndex = 0;
		private static int currentWaveIndex = 0;

		private static List<SpawnEvent> eventTimeline = new List<SpawnEvent>();

		#endregion
		#region Methods

		public static void initialize()
		{
			// Reset all values in order to start a new game session:
			currentLevel = null;
			currentWave = null;
			//currentLevelIndex = 0;

			if(eventTimeline == null)
			{
				eventTimeline = new List<SpawnEvent>();
			}
			eventTimeline.Clear();
		}

		public static void update()
		{
			if(eventTimeline.Count == 0) return;

			// If the time has come for the next timeline event:
			if(Time.time > eventTimeline[0].timestamp)
			{
				// Depending on event type, either spawn an enemy, start next wave or start next level:
				SpawnEvent timelineEvent = eventTimeline[0];
				switch (timelineEvent.type)
				{
				case SpawnEventType.SpawnEnemy:
					spawnEnemy(timelineEvent.enemyPrefab);
					break;
				case SpawnEventType.NextWave:
					startWave(timelineEvent.nextWave);
					// TODO
					break;
				case SpawnEventType.NextLevel:
					// TODO
					break;
				default:
					Debug.LogError("wtf");
					break;
				}

				// Purge event from timeline:
				eventTimeline.RemoveAt(0);
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

			// Load level asset from resources:
			Level newLevel = Resources.Load<Level>(newLevelName);
			if(newLevel == null)
			{
				Debug.LogError("EnemySpawner: Error! Could not find or load level resource: " + newLevelName);
				return false;
			}

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
			currentWave = null;
			currentWaveIndex = 0;

			// Iterate through the level's waves:
			float timestamp = Time.time;
			foreach(Wave wave in currentLevel.waves)
			{
				timestamp += currentLevel.waveInterval;

				// Add each wave as a new wave start event to timeline:
				SpawnEvent waveEvent = new SpawnEvent();
				waveEvent.type = SpawnEventType.NextWave;
				waveEvent.nextWave = wave;
				waveEvent.timestamp = timestamp;

				eventTimeline.Add(waveEvent);

				// Next, add the wave's spawn events to timeline:
				foreach(WaveSpawnEvent wse in wave.enemies)
				{
					Enemy enemy = wse.enemy;
					timestamp += wse.spawnInterval;

					SpawnEvent spawnEvent = new SpawnEvent();
					spawnEvent.type = SpawnEventType.SpawnEnemy;
					spawnEvent.enemyPrefab = enemy;
					spawnEvent.timestamp = timestamp;

					eventTimeline.Add(spawnEvent);
				}
			}

			return true;
		}
		
		private static void spawnEnemy(Enemy enemyPrefab)
		{
			if(enemyPrefab == null)
			{
				Debug.LogError("EnemySpawner: Error! Unable to spawn null enemy prefab!");
				return;
			}

			// Fetch next enemy prefab, then spawn it in scene:
			GameObject enemyGO = MonoBehaviour.Instantiate(enemyPrefab.gameObject) as GameObject;
			Enemy enemy = enemyGO.GetComponent<Enemy>();

			//Debug.Log("TEST: Spawned enemy: " + enemy.name);

			// Register newly spawned enemy with the ingame submachine:
			StatemachineStateIngame ingameState = Statemachine.CurrentState as StatemachineStateIngame;
			if(ingameState != null)
			{
				ingameState.Enemies.Add(enemy);
			}
		}

		private static void startWave(Wave newWave)
		{
			Debug.Log("TEST: Starting wave " + currentWaveIndex + " (" + newWave.name + ")");

			currentWave = newWave;
			currentWaveIndex++;

			//...
		}

		#endregion
	}
}
