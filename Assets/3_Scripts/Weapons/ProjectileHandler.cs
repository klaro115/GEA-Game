using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;

namespace Game.Weapons
{
	/// <summary>
	/// Static class for spawning, managing and simulating cannon and machinegun shots.
	/// </summary>
	public static class ProjectileHandler
	{
		#region Fields

		private static Projectile[] projectiles = null;
		private static RaycastHit2D[] results = null;
		private static Rocket[] rockets = null;

		private static Transform projectileParent = null;

		private static ContactFilter2D contactFilterPlayer;
		private static ContactFilter2D contactFilterEnemy;

		// Layer masks used in projectile collision detection:
		private static LayerMask layerMaskPlayer;
		private static LayerMask layerMaskEnemy;

		// Names of layers used in collision layer masks:
		private static readonly string[] layersPlayer = new string[]
		{ "Default", "TransparentFX", "Water", "Enemy" };
		private static readonly string[] layersEnemy = new string[]
		{ "Default", "TransparentFX", "Water", "Player" };

		// Resource and scene instance names:
		private static readonly string projectileParentName = "Projectiles";
		private static readonly string projectilePhysBodyPrefabName = "ProjectilePhysicalBody";
		private static readonly string rocketPrefabName = "Rocket";

		// Visual styles for projectiles based on owner:
		private static readonly Color projectileColorPlayer = new Color(1,0.882f,0.796f,1.0f);
		private static readonly Color projectileColorEnemy = new Color(1,0.2f,0.2f,1);

		#endregion
		#region Methods

		public static void initialize(int maxProjectileCount = 200, int maxRocketCount = 50)
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
			rockets = new Rocket[maxRocketCount];

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

			// Load rocket prefab from resources:
			Rocket rocketPrefab = Resources.Load<Rocket>(rocketPrefabName);

			// Spawn and initialize all rockets in pool:
			for(int i = 0; i < rockets.Length; ++i)
			{
				// Spawn a new instance of prefab in scene:
				GameObject instanceGO = MonoBehaviour.Instantiate(rocketPrefab.gameObject, projectileParent) as GameObject;
				instanceGO.SetActive(false);

				// Get rocket projectile component and write it to array:
				Rocket instance = instanceGO.GetComponent<Rocket>();
				rockets[i] = instance;
			}

			// Generate collision filters for player and enemy, so they can't accidentally hit their allies:
			contactFilterPlayer = new ContactFilter2D();
			contactFilterPlayer.useLayerMask = true;
			contactFilterPlayer.layerMask = LayerMask.GetMask(layersPlayer);
			contactFilterPlayer.useTriggers = true;

			contactFilterEnemy = new ContactFilter2D();
			contactFilterEnemy.useLayerMask = true;
			contactFilterEnemy.layerMask = LayerMask.GetMask(layersEnemy);
			contactFilterEnemy.useTriggers = true;
		}

    public static void clearProjectiles()
    {
      for (int i = 0; i < projectiles.Length; ++i)
      {
        disableProjectile(ref projectiles[i]);
      }
      foreach (Rocket r in rockets)
      {
        r.gameObject.SetActive(false);
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
			// Remove all active rocket instances from scene:
			if(rockets != null)
			{
				foreach(Rocket r in rockets)
				{
					if(r != null) MonoBehaviour.Destroy(r.gameObject);
				}
			}
			// Reset all member fields:
			projectiles = null;
			results = null;
			rockets = null;
		}

		/// <summary>
		/// Gizmos drawing method, for debugging.
		/// </summary>
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
		public static void spawnProjectile(Vector3 startPosition, Vector3 velocity, int damage, bool fromPlayer)
		{
			for(int i = 0; i < projectiles.Length; ++i)
			{
				Projectile p = projectiles[i];

				// Use the first 'dead' projectile slot you find:
				if(!p.isAlive)
				{
					p.isAlive = true;
					p.fromPlayer = fromPlayer;
					p.position = startPosition;
					p.velocity = velocity;
					p.damage = damage;
					if(p.physicalBody != null)
					{
						p.physicalBody.position = startPosition;
						p.physicalBody.gameObject.SetActive(true);

						Color pColor = fromPlayer ? projectileColorPlayer : projectileColorEnemy;
						p.physicalBody.GetComponent<SpriteRenderer>().color = pColor;
					}

					projectiles[i] = p;
					break;
				}
			}
		}

		public static bool spawnRocket(Vector3 startPosition, Vector3 direction, bool fromPlayer)
		{
			// Find an unused rocket in pool:
			for(int i = 0; i < rockets.Length; ++i)
			{
				Rocket rocket = rockets[i];
				if(rocket != null && !rocket.gameObject.activeSelf)
				{
					// Activate rocket and initialize behaviour:
					rocket.gameObject.SetActive(true);
					rocket.initialize(startPosition, direction, fromPlayer);

					return true;
				}
			}
			// No rocket available in pool, return false:
			return false;
		}

		public static ContactFilter2D getContactFilter(bool forPlayer)
		{
			return forPlayer ? contactFilterPlayer : contactFilterEnemy;
		}

		/// <summary>
		/// Update the projectile simulation, must be called every frame unless the game is paused.
		/// </summary>
		public static void update()
		{
			// Calculate camera area rectangle in world space: (used to kill expired projectiles)
			Camera cam = Camera.main;
			float camOrthoSize = cam.orthographicSize + 1.0f;
			Vector2 screenSize = new Vector2(camOrthoSize * cam.aspect, camOrthoSize);
			Rect screenAreaRect = new Rect(-screenSize, 2.0f * screenSize);

			if(Statemachine.IsIngame)
			{
				// Update normal dumb-fire bullets:
				updateProjectiles(screenAreaRect);
			}

			// Update homing rockets:
			updateRockets(screenAreaRect);
		}

		private static void updateProjectiles(Rect screenAreaRect)
		{
			// Iterate through projectiles array:
			for(int i = 0; i < projectiles.Length; ++i)
			{
				Projectile p = projectiles[i];

				// Skip any projectile slots that are not in a live state:
				if(!p.isAlive) continue;

				// Calculate position of the projectile:
				Vector2 nextPosition = p.position + p.velocity * Time.deltaTime;
				// Determine which collision filter to use based on who fired the projectile:
				ContactFilter2D filter = p.fromPlayer ? contactFilterPlayer : contactFilterEnemy;

				// Cast a ray from previous projectile position to updated position:
				if(Physics2D.Linecast(p.position, nextPosition, filter, results) > 0)
				{
					RaycastHit2D hit = results[0];

					// Tell the body that was hit to receive an amount of damage:
					executeProjectileHit(p, hit);

					// Disable the projectile:
					disableProjectile(ref p);
				}
				// If nothing was hit, update projectile position and move physical representation as well:
				else
				{
					p.position = nextPosition;
					if(p.physicalBody != null) p.physicalBody.position = nextPosition;

					// Check if the projectile has left the screen:
					if(!screenAreaRect.Contains(p.position))
					{
						// Disable the projectile:
						disableProjectile(ref p);
					}
				}

				// Write changed struct values back to array:
				projectiles[i] = p;
			}
		}
		private static void updateRockets(Rect screenAreaRect)
		{
			// Update rockets:
			foreach(Rocket rocket in rockets)
			{
				if(!rocket.gameObject.activeSelf) continue;

				rocket.update();

				// Destroy rocket after it exits the screen area:
				if(!screenAreaRect.Contains(rocket.transform.position))
				{
					rocket.gameObject.SetActive(false);
				}
			}
		}

		private static void executeProjectileHit(Projectile projectile, RaycastHit2D rayHit)
		{
			// Determine target GO and the amount of damage to be dealt:
			GameObject target = rayHit.transform.gameObject;
			int damage = projectile.damage;

			// Debug.Log("TEST: Target was hit: " + target.name);

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
