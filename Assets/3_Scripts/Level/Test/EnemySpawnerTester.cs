using UnityEngine;
using System.Collections;

namespace Game
{
	/// <summary>
	/// Simple testing helper class for initializing and updating the enemy spawner static class.
	/// </summary>
	public class EnemySpawnerTester : MonoBehaviour
	{
		#region Fields

		public Level testLevel = null;

		#endregion
		#region Methods

		void Start ()
		{
			EnemySpawner.initialize();
			EnemySpawner.setLevel(testLevel);
		}
		
		void Update ()
		{
			EnemySpawner.update();
		}

		#endregion
	}
}
