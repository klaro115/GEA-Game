﻿using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Rocket : MonoBehaviour, IDamageReceiver
	{
		#region Fields

		private Rigidbody2D rig = null;
		private Character target = null;

		public bool fromPlayer = false;
		public int damage = 1;

		public float speed = 6.0f;
		public float rotationSpeed = 60.0f;

		// TODO: Add explosion effect prefab.

		#endregion
		#region Fields Static

		private static readonly string layerNamePlayer = "Player";
		private static readonly string layerNameEnemy = "Enemy";

		#endregion
		#region Methods
		
		public void initialize(Vector3 position, Vector3 direction, bool inFromPlayer)
		{
			rig = GetComponent<Rigidbody2D>();

			// Adjust rocket's position and orientation:
			transform.position = position;
			transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

			// Set some other values:
			fromPlayer = inFromPlayer;

			// Set the rocket's layer to match its 'owner': (used in collision detection)
			gameObject.layer = LayerMask.NameToLayer(fromPlayer ? layerNamePlayer : layerNameEnemy);

			// Find a new homing target:
			StatemachineStateIngame state = StatemachineStateIngame.getStatemachine();
			if(fromPlayer)
			{
				float smallestAngle = 180.0f;
				Enemy smallestAngleEnemy = null;

				// Find an enemy that is directly in the rocket's path:
				foreach(Enemy enemy in state.Enemies.ToArray())	// TODO: Need to unregister enemies on death!!!
				{
					if(enemy == null) continue;

					Vector2 enemyDir = enemy.transform.position - transform.position;
					float enemyDot = Mathf.Abs(Vector2.Dot(transform.right, enemyDir));
					if(enemyDot < smallestAngle)
					{
						smallestAngle = enemyDot;
						smallestAngleEnemy = enemy;
					}
				}
				target = smallestAngleEnemy;
			}
			else
			{
				// Use player character as target:
				target = state.Player;
			}
		}

		public void update()
		{
			if(!Statemachine.IsIngame)
			{
				rig.velocity = Vector2.zero;
				return;
			}

			// Get a flight direction from target:
			Vector2 targetDirection = transform.forward;
			if(target != null)
			{
				targetDirection = target.transform.position - transform.position;
			}

			// Rotate the rocket to face the given target direction:
			float directionDot = Vector2.Dot(transform.right, targetDirection);
			float rotationFactor = Mathf.Clamp(directionDot, -1.0f, 1.0f);
			transform.Rotate(0, 0, -rotationFactor * rotationSpeed * Time.deltaTime);

			// Make the rocket move forward at all times:
			rig.velocity = transform.up * speed;
		}

		/// <summary>
		/// Damage receiver method, called if hit by any projectile or rocket.
		/// </summary>
		/// <param name="damage">Amount of damage received.</param>
		public void applyDamage(int damageDealt)
		{
			selfDestruct();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			// Figure out which object to deal damage to:
			GameObject target = collider.gameObject;

			// Ignore inflicting damage on allied contacts:
			if(target.layer == gameObject.layer)
			{
				return;
			}

			// Deal damage.
			int finalDamage = fromPlayer ? damage * 10 : damage;
			target.SendMessage("applyDamage", finalDamage, SendMessageOptions.DontRequireReceiver);

			// Destroy self:
			selfDestruct();
		}

		private void selfDestruct()
		{
			// TODO: Spawn an explosion in the rocket's current position.

			// Fake a destruct on a hit, by simply deactivating this gameObject:
			gameObject.SetActive(false);
		}

		#endregion
	}
}
