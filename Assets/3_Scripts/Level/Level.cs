using System.Collections;
using UnityEngine;

namespace Game
{
	[CreateAssetMenu(menuName="Create Level", fileName="new Level")]
	[System.Serializable]
	public class Level : ScriptableObject
	{
		#region Fields

		public Wave[] waves;				// Waves in this level in chronological order of appearance.
		public float waveInterval = 1.0f;	// Time interval between the end of one wave and the start of another.
		public int completionBonus = 20;	// Bonus points for completing this level.

		#endregion
	}
}
