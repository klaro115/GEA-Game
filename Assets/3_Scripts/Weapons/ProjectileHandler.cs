using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
	public static class ProjectileHandler
	{
		#region Fields

		private static Projectile[] projectiles = null;
		private static RaycastHit2D[] results = null;

		private static Transform projectileParent = null;

		private static readonly string projectileParentName = "Projectiles";
		private static readonly string projectilePhysBodyPrefabName = "ProjectilePhysicalBody";

		#endregion
		#region Methods

		public static void initialize(int maxProjectileCount = 200)
		{
			// Make sure at least 1 projectile can be simulated at any time:
			if(maxProjectileCount < 1)
			{
				Debug.LogError("ProjectileHandler: Error! Invalid projectile count value: " +
					maxProjectileCount + ". Setting paramter to fallback value instead.");
				maxProjectileCount = 10;
			}

			// Initialize and allocate arrays:
			projectiles = new Projectile[maxProjectileCount];
			results = new RaycastHit2D[1];

			// Prepare scene elements and make sure there's a parent GO in scene for housing all physical bodies:
			GameObject projectileParentGO = GameObject.Find(projectileParentName);
			if(projectileParentGO == null)
			{
				projectileParent = new GameObject(projectileParentName).transform;
			}
			else
			{
				projectileParent = projectileParentGO.transform;
			}
			// Load resources for visualizing projectiles in scene:
			GameObject physBodyPrefab = Resources.Load<GameObject>(projectilePhysBodyPrefabName);

			// Initialize all projectiles:
			for(int i = 0; i < projectiles.Length; ++i)
			{
				Projectile p = Projectile.Blank;

				// Spawn a visible 'physical' game object representing the projectile instance in scene:
				if(physBodyPrefab != null)
				{
					GameObject newPhysBody = MonoBehaviour.Instantiate(physBodyPrefab, projectileParent) as GameObject;
					newPhysBody.SetActive(false);
					p.physicalBody = newPhysBody.transform;
				}

				projectiles[i] = p;
			}
		}

		public static void shutdown()
		{
			// Remove all projectiles' physical representations from scene:
			if(projectiles != null)
			{
				foreach(Projectile p in projectiles)
				{
					if(p.physicalBody != null)
					{
						MonoBehaviour.Destroy(p.physicalBody.gameObject);
					}
				}
			}
			// Reset all member fields:
			projectiles = null;
			results = null;
		}

		public static void drawGizmos()
		{
			Gizmos.color = Color.red;
			Vector3 size = Vector3.one * 0.05f;
			if(projectiles != null)
			{
				foreach(Projectile p in projectiles)
				{
					if(p.isAlive) Gizmos.DrawWireCube(p.position, size);
				}
			}
		}

		/// <summary>
		/// Spawn a new projectile at a given position and moving in a direction.
		/// </summary>
		/// <param name="startPosition">Start position, from where it is fired.</param>
		/// <param name="velocity">Velocity of the projectile.</param>
		/// <param name="damage">Damage dealt to anything hit by the projectile.</param>
		/// <param name="isPlayer">Wether this was fired by the player or the enemy.</param>
		public static void spawnProjectile(Vector3 startPosition, Vector3 velocity, int damage)//, bool isPlayer)
		{
			for(int i = 0; i < projectiles.Length; ++i)
			{
				Projectile p = projectiles[i];

				// Use the first 'dead' projectile slot you find:
				if(!p.isAlive)
				{
					p.isAlive = true;
					p.position = startPosition;
					p.velocity = velocity;
					p.damage = damage;
					if(p.physicalBody != null)
					{
						p.physicalBody.position = startPosition;
						p.physicalBody.gameObject.SetActive(true);
					}

					projectiles[i] = p;
					break;
				}
			}
		}

		public static void update()
		{
			ContactFilter2D filter = new ContactFilter2D();

			// Iterate through projectiles array:
			for(int i = 0; i < projectiles.Length; ++i)
			{
				Projectile p = projectiles[i];

				// Skip any projectile slots that are not in a live state:
				if(!p.isAlive) continue;

				// Calculate position of the projectile:
				Vector2 nextPosition = p.position + p.velocity * Time.deltaTime;
				// Cast a ray from previous projectile position to updated position:
				if(Physics2D.Linecast(p.position, nextPosition, filter, results) > 0)
				{
					// Tell the body that was hit to receive an amount of damage:
					RaycastHit2D hit = results[0];
					executeProjectileHit(p, hit);

					// Disable the projectile:
					disableProjectile(ref p);
				}
				// If nothing was hit, update projectile position and move physical representation as well:
				else
				{
					p.position = nextPosition;
					if(p.physicalBody != null) p.physicalBody.position = nextPosition;

					// TODO: Check if the projectile has left the screen, disable if if that's the case!
					/*
					if(...)
					{
						// Disable the projectile:
						disableProjectile(ref p);
					}
					*/
				}

				// Write changed struct values back to array:
				projectiles[i] = p;
			}
		}

		private static void executeProjectileHit(Projectile projectile, RaycastHit2D rayHit)
		{
			// Determine target GO and the amount of damage to be dealt:
			GameObject target = rayHit.transform.gameObject;
			int damage = projectile.damage;

			Debug.Log("TEST: Target was hit: " + target.name);

			// Send a message to target, calling a method named 'applyDamage', if present:
			target.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}

		private static void disableProjectile(ref Projectile projectile)
		{
			// Reset the projectile's 'alive' status flag:
			projectile.isAlive = false;

			// Deactivate the physical representation, if present:
			if(projectile.physicalBody != null)
			{
				projectile.physicalBody.gameObject.SetActive(false);
			}
		}

		#endregion
	}
}
