using UnityEngine;
using System.Collections;
using Game.Weapons;

namespace Game
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Player : Character
	{
		#region Fields

		private Rigidbody2D rig;

		public float damageCooldown = 0.1f;	// Minimum time span where damage is ignored after taking a hit.
		private float lastDamageReceivedTime = 0.0f;	// Time when the player last took damage.

		#endregion
		#region Methods

		protected override void Start ()
		{
      rig = GetComponent<Rigidbody2D>();

      
      // Apply scale based on radius
      Vector3 size = new Vector3(radius * 2, radius * 2, radius * 2);
      this.transform.localScale = size;

			//mainWeapon = null;
			//secWeapon = null;
      calcScreenspace(radius * 2);
    }

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;

      // Check if dead
      // TODO: Show GameOver Screen and stop game
      //if (isDead()) Destroy(this.gameObject);  // NOTE: commented this out, for testing purposes.

			// Fetch input signals:
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			// Move the ship:
			//transform.Translate(new Vector3(x, y, 0) * baseSpeed * Time.deltaTime);
			rig.velocity = new Vector3(x, y, 0) * baseSpeed;

			// Limit position to screen space:
			Vector3 pos = transform.position;
			pos.x = Mathf.Clamp(pos.x, -screenSpace.x, screenSpace.x);
			pos.y = Mathf.Clamp(pos.y, -screenSpace.y, screenSpace.y);
			transform.position = pos;

			if(mainWeapon != null && Input.GetKey(KeyCode.Space))
			{
				mainWeapon.fire();
			}
		}

    protected bool isDead()
    {
      return this.hitpoints <= 0;
    }
	
	public override void applyDamage(int dmg)
    {
      // Only allow player to repeatedly take damage after a cooldown period has passed:
      if(Time.time > damageCooldown + lastDamageReceivedTime)
      {
        lastDamageReceivedTime = Time.time;
        Debug.Log("Player Hit! " + this.hitpoints + " left.");
        this.hitpoints -= dmg;
      }
    }

		void OnCollisionEnter2D(Collision2D collision)
		{
			// Define some constants for dealing collision damage:
			const int targetCollisionDamage = 999;
			const int playerCollisionDamage = 1;

			// Get the other party we collided with:
			GameObject target = collision.gameObject;
			// Don't apply collision damage between allied characters:
			if(target.layer == gameObject.layer)
			{
				return;
				// NOTE: This may be needed in later stages, when dealing with other players or AI companions.
			}

			// Apply damage to collision target:
			target.SendMessage("applyDamage", targetCollisionDamage, SendMessageOptions.DontRequireReceiver);

			// Apply collision damage to self:
			applyDamage(playerCollisionDamage);
		}

		#endregion
	}
}
