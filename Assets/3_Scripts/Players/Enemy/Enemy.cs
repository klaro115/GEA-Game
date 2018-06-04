using UnityEngine;
using System.Collections;

namespace Game
{
	public class Enemy : Character
	{
		#region Fields
		// The moving path for this enemy
		public Vector2[] flightBehaviour = new Vector2[2] { new Vector2(-1,1), new Vector2(1,-1) };
		// The points the player receives when this enemy is destroyed
		public int points = 100;
		// The rate of powerUp items to be dropped
		public float dropRate = 0.05f;
		// The current checkpoint index (of flightBehaviour)
		protected int checkpointIndex = 0;
		// The index for the last Checkpoint
		protected int lastCheckpointIndex;
		// The min. distance to a checkpoint to mark it as reached
		protected float checkpointReachedDistance = 0.5f;
		#endregion


		#region Methods

		protected override void Start ()
		{

		}

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;

			if (checkpointReached()) checkpointIndex++;
			if(isDead()) Destroy(this.gameObject);
			else move();
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

		protected bool checkpointReached()
		{
			return (((Vector2)transform.position - flightBehaviour[checkpointIndex + 1]).magnitude < checkpointReachedDistance);
		}

		protected void move()
		{
			Vector2 flightDirection = (flightBehaviour[checkpointIndex + 1] - flightBehaviour[checkpointIndex]).normalized;
			transform.position += baseSpeed * (Vector3)flightDirection * Time.deltaTime;
		}

		protected bool isDead()
		{
			return (checkpointIndex >= lastCheckpointIndex);
		}

		#endregion
	}
}
