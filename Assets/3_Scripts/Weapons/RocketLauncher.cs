using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public class RocketLauncher : Weapon
	{
		#region Fields

		private float lastShotTime = -1.0f;

		#endregion
		#region Methods

		public override void fire ()
		{
			float gameTime = StatemachineStateIngame.getStatemachine().GameTime;

			// Allow firing again after a minimum time interval between shots has passed:
			if(gameTime < lastShotTime + fireInterval) return;

			lastShotTime = gameTime;

			// Request projectile handler to spawn and simulate a new rocket instance:
			ProjectileHandler.spawnRocket(transform.position, transform.up, playerControlled);
		}

		#endregion
	}
}
