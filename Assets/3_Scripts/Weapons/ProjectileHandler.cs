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

		#endregion
		#region Methods

		public static void initialize()
		{
			// Initialize and allocate arrays:
			projectiles = new Projectile[100];
			results = new RaycastHit2D[1];

			// Prepare scene elements and load resources for visualizing projectiles in scene:
			projectileParent = new GameObject("Projectiles").transform;
			GameObject physBodyPrefab = Resources.Load<GameObject>("ProjectilePhysicalBody");

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

		// TODO: Add a shutdown or destroy method, clearing all projectile instances and stuff.

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

			for(int i = 0; i < projectiles.Length; ++i)
			{
				Projectile p = projectiles[i];

				if(!p.isAlive) continue;

				Vector2 nextPosition = p.velocity * Time.deltaTime;
				if(Physics2D.Linecast(p.position, nextPosition, filter, results) > 0)
				{
					// Tell the body that was hit to receive an amount of damage:
					RaycastHit2D hit = results[0];
					hit.transform.gameObject.SendMessage("ApplyDamage", p.damage, SendMessageOptions.DontRequireReceiver);

					// Disable the projectile:
					p.isAlive = false;
					if(p.physicalBody != null) p.physicalBody.gameObject.SetActive(false);
				}
				else
				{
					p.position = nextPosition;
					if(p.physicalBody != null) p.physicalBody.position = nextPosition;
				}

				projectiles[i] = p;
			}
		}

		#endregion
	}
}
