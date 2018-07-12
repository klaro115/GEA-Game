using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public abstract class Weapon : MonoBehaviour
	{
		#region Fields

		public float fireInterval;
		public WeaponModifier modifier = WeaponModifier.Default;

    protected AudioSource audioSource = null;

    #endregion
    #region Methods

    private void Awake()
    {
      this.initAudioSource();
    }

    virtual protected void initAudioSource()
    {
      audioSource = transform.GetComponentInParent<AudioSource>();
    }
    public abstract void fire();

		#endregion
	}
}
