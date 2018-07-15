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

		private static readonly float scatterFiringAngle = 10.0f;

		public SpriteRenderer beamEffectPrefab = null;
		private SpriteRenderer[] beamEffects = null;
		public float beamEffectExtraLength = 2.5f;

		private RaycastHit2D[] rayHits = new RaycastHit2D[1] { new RaycastHit2D() };
		private ContactFilter2D contactFilter;

    AudioSource audioSource = null;

    #endregion
    #region Methods

    private void Awake()
    {
      initAudioSource();

			initBeamEffects();
    }

    protected void initAudioSource()
    {
      // Set an extra audiosource
      audioSource = transform.GetComponent<AudioSource>();
      audioSource.clip = Resources.Load<AudioClip>("laser-shot");
      audioSource.loop = true;
    }
		private void initBeamEffects()
		{
			if(beamEffectPrefab != null)
			{
				beamEffectPrefab.gameObject.SetActive(true);

				const int beamEffectCount = 11;
				beamEffects = new SpriteRenderer[beamEffectCount];
				for(int i = 0; i < beamEffectCount; ++i)
				{
					GameObject bfxGO = Instantiate(beamEffectPrefab.gameObject, transform) as GameObject;
					bfxGO.transform.position = beamEffectPrefab.transform.position;

					beamEffects[i] = bfxGO.GetComponent<SpriteRenderer>();
				}

				beamEffectPrefab.gameObject.SetActive(false);
				disableBeamEffects(0);
			}
		}

    void LateUpdate()
		{
			float gameTime = StatemachineStateIngame.getStatemachine().GameTime;

			if(gameTime > lastBlastTime + fireInterval && beamEffects != null)
			{
        audioSource.Stop();
				disableBeamEffects(0);
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

			contactFilter = ProjectileHandler.getContactFilter(playerControlled);

			// Call different firing behaviours depending on modifier type.
			switch (modifier.type)
			{
			case WeaponModifierType.Default:
				fireSingleShot(transform.position, transform.up, 0);
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
			int j = 0;	// Shot index counter.

			Vector3 muzzlePosition = transform.position;
			Vector3 muzzleDirection = transform.up;

			// Fire a single shot in a forward direction:
			fireSingleShot(muzzlePosition, muzzleDirection, j++);

			// Fire an additional pair of 2 shots per modifier level:
			int angleCount = modifier.level + 1;
			for(int i = 1; i < angleCount; ++i)
			{
				// Create a rotation of 'angle' degrees:
				float angle = i * scatterFiringAngle;
				Quaternion rotationL = Quaternion.AngleAxis(angle, Vector3.forward);
				Quaternion rotationR = Quaternion.AngleAxis(-angle, Vector3.forward);

				// Rotate muzzle direction:
				Vector3 shotDirectionL = rotationL * muzzleDirection;
				Vector3 shotDirectionR = rotationR * muzzleDirection;

				// Fire two individual shots:
				fireSingleShot(muzzlePosition, shotDirectionL, j++);
				fireSingleShot(muzzlePosition, shotDirectionR, j++);
			}

			disableBeamEffects(j);
		}

		private void fireCrossShot()
		{
			Vector3 muzzlePosition = transform.position;
			int i = 0;	// Shot index counter.

			// Fire one shot in every cardinal direction:
			fireSingleShot(muzzlePosition, transform.up, i++);
			fireSingleShot(muzzlePosition, -transform.up, i++);
			fireSingleShot(muzzlePosition, transform.right, i++);
			fireSingleShot(muzzlePosition, -transform.right, i++);

			// Fire an additional 4 shots on higher levels:
			if(modifier.level > 1)
			{
				Vector3 dir0 = transform.TransformDirection(new Vector3(-0.707f,0.707f,0));
				Vector3 dir1 = transform.TransformDirection(new Vector3(-0.707f,-0.707f,0));
				Vector3 dir2 = transform.TransformDirection(new Vector3(0.707f,0.707f,0));
				Vector3 dir3 = transform.TransformDirection(new Vector3(0.707f,-0.707f,0));

				fireSingleShot(muzzlePosition, dir0, i++);
				fireSingleShot(muzzlePosition, dir1, i++);
				fireSingleShot(muzzlePosition, dir2, i++);
				fireSingleShot(muzzlePosition, dir3, i++);
			}

			disableBeamEffects(i);
		}

		private void fireSingleShot(Vector3 position, Vector3 direction, int shotIndex)
		{
			float beamLength = range;
			int resCount = Physics2D.Raycast(position, direction, contactFilter, rayHits, range);
			if(resCount != 0)
			{
				RaycastHit2D hit = rayHits[0];

				beamLength = hit.distance;

				GameObject targetGO = hit.collider.gameObject;
				targetGO.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
			}

			if(beamEffects != null && shotIndex < beamEffects.Length)
			{
				SpriteRenderer beamEffect = beamEffects[shotIndex];
				beamLength += beamEffectExtraLength;

				beamEffect.enabled = true;
				beamEffect.size = new Vector2(beamEffect.size.x, beamLength);
				beamEffect.transform.up = direction;
			}
		}

		private void disableBeamEffects(int startIndex)
		{
			if(beamEffects == null) return;

			for(int i = startIndex; i < beamEffects.Length; ++i)
			{
				beamEffects[i].enabled = false;
			}
		}


		#endregion
	}
}