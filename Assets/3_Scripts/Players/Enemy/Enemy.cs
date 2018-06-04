using UnityEngine;
using System.Collections;

namespace Game
{
	public class Enemy : Character
	{
		#region Fields
		public Vector2[] flightBehaviour = new Vector2[2] { new Vector2(-1,1), new Vector2(1,-1) };
		public int points = 100;
		public float dropPercentage = 0.05f;
		public int checkpointIndex = 0;
		#endregion


		#region Methods

		protected override void Start ()
		{

		}

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;
		}

		/**
			*	Translates Coordinates of flightBehaviour ([-1,1] [-1,1]) into screenspace.
			*
			*/
		protected void translateFlightCoords()
		{
			// Translate Flightbehaviour params to screenspace
			for(int i = 0; i < flightBehaviour.Length; ++i) {
			  flightBehaviour[i].x = flightBehaviour[i].x * screenSpace.x;
			  flightBehaviour[i].y = flightBehaviour[i].y * screenSpace.y;
			}
		}

		#endregion
	}
}
