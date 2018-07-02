using System.Collections;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct WaveSpawnEvent
	{
		public Enemy enemy;						// Enemy prefab to spawn during this event in scene.
		public FlightBehaviour flightBehaviour;	// Flight behaviour tto assign to the enemy.	
		public float spawnInterval;				// Time delay between spawning enemy instances in scene.
	}

	[CreateAssetMenu(menuName="Create Wave prefab", fileName="new Wave")]
	[System.Serializable]
	public class Wave : ScriptableObject
	{
		#region Fields

		public WaveSpawnEvent[] enemies;	// Spawn events of the wave, in chronological order.

		#endregion
	}
}
