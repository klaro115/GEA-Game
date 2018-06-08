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

		#endregion
		#region Methods

		public override void fire ()
		{
			// Allow firing again after a minimum time interval between shots has passed:
			if(Time.time < lastShotTime + fireInterval) return;

			lastShotTime = Time.time;

			// TODO: Wrap this into a switch-case, calling different behaviours depending on modifier type.

			// Get origin/source position and shooting direction:
			Vector3 muzzlePosition = transform.position;
			Vector3 muzzleDirection = transform.up;
			Vector3 muzzleVelocity = muzzleDirection * projectileSpeed;

			// Tell the projectile handler to spawn a single shot:
			ProjectileHandler.spawnProjectile(muzzlePosition, muzzleVelocity, damage);
		}

		#endregion
	}
}
