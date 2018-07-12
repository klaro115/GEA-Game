using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public abstract class Weapon : MonoBehaviour
	{
		#region Fields

		public float fireInterval;
		public WeaponModifier modifier = WeaponModifier.Default;

<<<<<<< HEAD
    protected AudioSource audioSource = null;

    #endregion
    #region Methods

    private void Awake()
    {
      this.initAudioSource();
    }
=======
		public bool playerControlled = false;

		#endregion
		#region Methods
>>>>>>> dcaf4b75b879eeadd551d982cdff0d7fc27db4fe

    virtual protected void initAudioSource()
    {
      audioSource = transform.GetComponentInParent<AudioSource>();
    }
    public abstract void fire();

		#endregion
	}
}
