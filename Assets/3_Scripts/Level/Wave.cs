using System.Collections;
using UnityEngine;

namespace Game
{
	[CreateAssetMenu(menuName="Create Wave prefab", fileName="new Wave")]
	[System.Serializable]
	public class Wave : ScriptableObject
	{
		#region Fields

		public Enemy[] enemies;		// Enemy prefabs of the wave, in chronological order.
		public float spawnInterval;	// Time delay between spawning enemy instances in scene.

		[System.NonSerialized]
		public int currentSpawnIndex;

		#endregion
	}
}
