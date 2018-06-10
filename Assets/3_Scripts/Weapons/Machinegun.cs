using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public class Machinegun : Weapon
	{
		#region Fields

		private float lastShotTime = -1.0f;
		[Range(1, 20)]	// note: feel free to adjust this value range.
		public int damage = 1;
		public float projectileSpeed = 10.0f;

		public bool playerControlled = false;

		#endregion
		#region Methods

		public override void fire ()
		{
			float gameTime = StatemachineStateIngame.getStatemachine().GameTime;

			// Allow firing again after a minimum time interval between shots has passed:
			if(gameTime < lastShotTime + fireInterval) return;

			lastShotTime = gameTime;

			// Call different firing behaviours depending on modifier type.
			switch (modifier.type)
			{
			case WeaponModifierType.Default:
				fireSingleShot(transform.position, transform.up);
				break;
			case WeaponModifierType.ScatterShot:
				fireScatterShot();
				break;
			case WeaponModifierType.CrossShot:
				fireCrossShot();
				break;
			default:
				Debug.LogError("Machinegun: Error! Unknown weapon modifier type: " + modifier.type.ToString());
				break;
			}
		}

		private void fireScatterShot()
		{
			// Base angle between scattered shot trajectory lines:
			const float firingAngle = 10.0f;

			Vector3 muzzlePosition = transform.position;
			Vector3 muzzleDirection = transform.up;

			// Fire a single shot in a forward direction:
			fireSingleShot(muzzlePosition, muzzleDirection);

			// Fire an additional pair of 2 shots per modifier level:
			int angleCount = modifier.level + 1;
			for(int i = 1; i < angleCount; ++i)
			{
				// Create a rotation of 'angle' degrees:
				float angle = i * firingAngle;
				Quaternion rotationL = Quaternion.AngleAxis(angle, Vector3.forward);
				Quaternion rotationR = Quaternion.AngleAxis(-angle, Vector3.forward);

				// Rotate muzzle direction:
				Vector3 shotDirectionL = rotationL * muzzleDirection;
				Vector3 shotDirectionR = rotationR * muzzleDirection;

				// Fire two individual shots:
				fireSingleShot(muzzlePosition, shotDirectionL);
				fireSingleShot(muzzlePosition, shotDirectionR);
			}
		}

		private void fireCrossShot()
		{
			Vector3 muzzlePosition = transform.position;

			// Fire one shot in every cardinal direction:
			fireSingleShot(muzzlePosition, transform.up);
			fireSingleShot(muzzlePosition, -transform.up);
			fireSingleShot(muzzlePosition, transform.right);
			fireSingleShot(muzzlePosition, -transform.right);

			// Fire an additional 4 shots on higher levels:
			if(modifier.level > 1)
			{
				Vector3 dir0 = transform.TransformDirection(new Vector3(-0.707f,0.707f,0));
				Vector3 dir1 = transform.TransformDirection(new Vector3(-0.707f,-0.707f,0));
				Vector3 dir2 = transform.TransformDirection(new Vector3(0.707f,0.707f,0));
				Vector3 dir3 = transform.TransformDirection(new Vector3(0.707f,-0.707f,0));

				fireSingleShot(muzzlePosition, dir0);
				fireSingleShot(muzzlePosition, dir1);
				fireSingleShot(muzzlePosition, dir2);
				fireSingleShot(muzzlePosition, dir3);
				// TODO: Revisit/Change behaviour on higher levels?
			}
		}

		private void fireSingleShot(Vector3 position, Vector3 direction)
		{
			// Calculate muzzle velocity from direction and speed:
			Vector3 velocity = direction * projectileSpeed;

			// Tell the projectile handler to spawn a single shot:
			ProjectileHandler.spawnProjectile(position, velocity, damage, playerControlled);
		}

		#endregion
	}
}
