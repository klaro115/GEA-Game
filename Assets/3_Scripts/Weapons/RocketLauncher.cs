using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public class RocketLauncher : Weapon
	{
		#region Fields

		private float lastShotTime = -1.0f;

    AudioClip soundRocketShot = null;

    #endregion
    #region Methods

    private void Start()
    {
      soundRocketShot = Resources.Load<AudioClip>("rocket-shot");
    }

    public override void fire ()
		{
			float gameTime = StatemachineStateIngame.getStatemachine().GameTime;

			// Allow firing again after a minimum time interval between shots has passed:
			if(gameTime < lastShotTime + fireInterval) return;

      SoundHandler.playOneShot(soundRocketShot);
			lastShotTime = gameTime;

			// Request projectile handler to spawn and simulate a new rocket instance:
			ProjectileHandler.spawnRocket(transform.position, transform.up, playerControlled);
		}

		#endregion
	}
}
