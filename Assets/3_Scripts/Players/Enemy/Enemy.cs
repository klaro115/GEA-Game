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
    protected Vector2 flightDirection;
    // TODO: Remove if Rotating the whole GameObject isnt necessary anymore
    protected GameObject player;
    #endregion


    #region Methods

    protected override void Start ()
		{
      // TODO: Remove if Rotating the whole GameObject isnt necessary anymore
      player = GameObject.Find("Player");

      calcScreenspace(radius * 2);
      // Apply scale based on radius
      Vector3 size = new Vector3(radius * 2, radius * 2, radius * 2);
      this.transform.localScale = size;

      // Calculate routes 
      translateFlightCoords();
      lastCheckpointIndex = flightBehaviour.Length - 1;
      transform.position = flightBehaviour[0];
    }

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;

			if (checkpointReached()) checkpointIndex++;
      if (hasReachedLastCheckpoint())
      {
        Destroy(this.gameObject);
      }
      else if (isDead())
      {
        // Drop items
        Destroy(this.gameObject);
      }
      else
      {
        move();
        if (this.mainWeapon != null) this.mainWeapon.fire();
      }
		}

    private void OnDestroy()
    {
        //Debug.Log("Enemy Destroyed");
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

    // Checks whether a checkpoint is reached
		protected bool checkpointReached()
		{
			return (((Vector2)transform.position - flightBehaviour[checkpointIndex + 1]).magnitude < checkpointReachedDistance);
		}

		protected void move()
		{

			flightDirection = (flightBehaviour[checkpointIndex + 1] - flightBehaviour[checkpointIndex]).normalized;

      transform.position += baseSpeed * (Vector3)flightDirection * Time.deltaTime;


      // Rotate towards vec3
      // TODO:  Weapon should rotate towards players position
      //        TODO: Remove if Rotating the whole GameObject isnt necessary anymore
      Vector3 target = player.transform.position;
      alignToTarget(transform.position, target);
    }
        
    
    protected bool hasReachedLastCheckpoint()
    {
        return (checkpointIndex >= lastCheckpointIndex);
    }

    public override void applyDamage(int dmg)
    {
      this.hitpoints -= dmg;
      // Debug.Log(dmg + "Damage applied. " + this.hitpoints + "left");
    }
    #endregion
  }
}
