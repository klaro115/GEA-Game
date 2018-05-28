using UnityEngine;
using System.Collections;

namespace Game
{
	public class Enemy : Character
	{
		#region Fields
		public Vector2[] flightBehaviour = new Vector2[2] { new Vector2(0,0), new Vector2(1,1) };
		public int points = 100;
		public float dropPercentage = 0.05;

		#endregion
		#region Methods

		protected override void Start ()
		{}

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;
		}

		#endregion
	}
}
