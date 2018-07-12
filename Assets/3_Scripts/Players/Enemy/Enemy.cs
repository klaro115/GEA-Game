using UnityEngine;
using System.Collections;
using Game.Weapons;
using Game.Items;

namespace Game
{
	public class Enemy : Character
	{
    #region Fields
    // The moving path for this enemy
    //public Vector2[] flightBehaviour = new Vector2[2] { new Vector2(-1,1), new Vector2(1,-1) };
    public FlightBehaviour flightBehaviour = null;
		// The points the player receives when this enemy is destroyed
		public int points = 100;
		// The rate of powerUp items to be dropped
		public float dropRate = 0.5f;
		// The current checkpoint index (of flightBehaviour)
		protected int checkpointIndex = 0;
		// The index for the last Checkpoint
		protected int lastCheckpointIndex;
		// The min. distance to a checkpoint to mark it as reached
		protected float checkpointReachedDistance = 0.5f;
    protected Vector2 flightDirection;
    protected string[] itemPool = new string[] {"HITPOINTS_PLUS", "MOD_CROSS_PLUS", "MOD_SCATTER_PLUS", "TYPE_MG", "TYPE_LASER"};
    // TODO: Remove if Rotating the whole GameObject isnt necessary anymore
    protected GameObject player;
    #endregion


    #region Methods

    protected override void Start ()
		{
      player = StatemachineStateIngame.getStatemachine().Player.gameObject;

      calcScreenspace(radius * 2);
      // Apply scale based on radius
      Vector3 size = new Vector3(radius * 2, radius * 2, radius * 2);
      this.transform.localScale = size;

      // Calculate routes
      translateFlightCoords();
      lastCheckpointIndex = flightBehaviour.waypoints.Length - 1;
      transform.position = flightBehaviour.waypoints[0];
      Debug.Log(transform.position.ToString());
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
        // Spawn points item (coins)
        this.spawnPoints(this.points);
        // Check if an item will drop, an spawn a random item if true
        this.spawnRandomItem();
        this.itemRoll(this.dropRate);
      }
      else
      {
        move();
        fireMainWeapons();
        fireSecondaryWeapons();
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
			for(int i = 0; i < flightBehaviour.waypoints.Length; ++i) {
			  flightBehaviour.waypoints[i].x = flightBehaviour.waypoints[i].x * screenSpace.x;
			  flightBehaviour.waypoints[i].y = flightBehaviour.waypoints[i].y * screenSpace.y;
			}
		}

    // Checks whether a checkpoint is reached
		protected bool checkpointReached()
		{
			return (((Vector2)transform.position - flightBehaviour.waypoints[checkpointIndex + 1]).magnitude < checkpointReachedDistance);
		}

		protected void move()
		{

			flightDirection = (flightBehaviour.waypoints[checkpointIndex + 1] - flightBehaviour.waypoints[checkpointIndex]).normalized;

      transform.position += baseSpeed * (Vector3)flightDirection * Time.deltaTime;


      // Rotate towards vec3
      // TODO:  Weapon should rotate towards players position
      //        TODO: Remove if Rotating the whole GameObject isnt necessary anymore
      Vector3 target = player.transform.position;
      transform.rotation = alignToTarget(transform.position, target);
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

    private void spawnPoints(int value)
    {
      Item itemPointsPrefab = Resources.Load<Item>("Points");
      Item itemPoints = Instantiate(itemPointsPrefab, transform.position, Quaternion.identity);
      itemPoints.pointsValue = this.points;
      itemPoints.type = "POINTS";
      Destroy(this.gameObject);
    }

    /**
     * Determines wheter the enemys drops an item or not. Returns true if it drops an item. Returns false if it does not drop any items.
     */
    private bool itemRoll(float dropRate)
    {
      // Roll whether an item drops using the given dropRate as %
      double dropRoll = Random.Range(0.0f, 1.0f);
      if (dropRoll < 1.0 - dropRate) 
      {
        //Debug.Log("ITEMROLL: " + dropRoll);
        return false;
      }
      return true;
      
    }

    private void spawnRandomItem()
    {
      // Determine wheter this enemy drops an item.
      if (!this.itemRoll(this.dropRate)) return;
      // calculate index of the item to drop of itemPool
      int itemIndex = Random.Range(0, itemPool.Length - 1);
      // Get the item name from itemPool
      string itemName = itemPool[itemIndex];
      // Load the prefab of the item to spawn
      Item itemPrefab = Resources.Load<Item>(itemName);
      // Spawn the item
      Item item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
      item.type = itemName;
      item.pointsValue = 0;
      //Debug.Log("ITEMSPAWN: " + itemName);
    }
    #endregion
  }
}
