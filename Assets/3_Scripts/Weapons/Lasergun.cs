using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
  [RequireComponent(typeof(AudioSource))]
	public class Lasergun : Weapon
	{
		#region Fields

		private float lastBlastTime = -1.0f;
		[Range(1, 20)]	// note: feel free to adjust this value range.
		public int damage = 1;
		public float range = 20.0f;

		public SpriteRenderer beamEffect = null;
		public float beamEffectExtraLength = 2.5f;

		private RaycastHit2D[] rayHits = new RaycastHit2D[1] { new RaycastHit2D() };

    AudioSource audioSource = null;

    #endregion
    #region Methods

    private void Awake()
    {
      initAudioSource();
    }

    protected void initAudioSource()
    {
      // Set an extra audiosource
      audioSource = transform.GetComponent<AudioSource>();
      audioSource.clip = Resources.Load<AudioClip>("laser-shot");
      audioSource.loop = true;
    }

    void LateUpdate()
		{
			float gameTime = StatemachineStateIngame.getStatemachine().GameTime;

			if(gameTime > lastBlastTime + fireInterval && beamEffect != null)
			{
        audioSource.Stop();
				beamEffect.enabled = false;
			}
		}

		public override void fire ()
		{
      float gameTime = StatemachineStateIngame.getStatemachine().GameTime;

			// Allow firing again after a minimum time interval between shots has passed:
			if(gameTime < lastBlastTime + fireInterval) return;

      audioSource.Play();

      lastBlastTime = gameTime;

			Vector2 muzzlePos = transform.position;
			Vector2 muzzleDir = transform.up;

			ContactFilter2D contactFilter = ProjectileHandler.getContactFilter(playerControlled);

			float beamLength = range;
			int resCount = Physics2D.Raycast(muzzlePos, muzzleDir, contactFilter, rayHits, range);
			if(resCount != 0)
			{
				RaycastHit2D hit = rayHits[0];

				beamLength = hit.distance;

				GameObject targetGO = hit.collider.gameObject;
				targetGO.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
			}

			if(beamEffect != null)
			{
				beamLength += beamEffectExtraLength;

				beamEffect.enabled = true;
				beamEffect.size = new Vector2(beamEffect.size.x, beamLength);
			}
		}

		#endregion
	}
}