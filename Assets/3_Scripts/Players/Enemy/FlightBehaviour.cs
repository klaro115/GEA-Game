using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	[CreateAssetMenu(menuName="Create FlightBehaviour", fileName="new FlightBehaviour")]
	[System.Serializable]
	public class FlightBehaviour : ScriptableObject
	{
		#region Fields
		
		public Vector2[] waypoints = new Vector2[2] { new Vector2(-1,1), new Vector2(1,-1) };
		
		#endregion
	}
}
