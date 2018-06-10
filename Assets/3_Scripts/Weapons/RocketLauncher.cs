using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public class RocketLauncher : Weapon
	{
		#region Fields

		private float lastShotTime = -1.0f;

		public Rocket rocketPrefab = null;

		public bool playerControlled = false;


		#endregion
		#region Methods

		public override void fire ()
		{
			// Allow firing again after a minimum time interval between shots has passed:
			if(Time.time < lastShotTime + fireInterval) return;

			lastShotTime = Time.time;

			// Request projectile handler to spawn and simulate a new rocket instance:
			ProjectileHandler.spawnRocket(transform.position, transform.up, rocketPrefab, playerControlled);
		}

		#endregion
	}
}
