using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public abstract class Weapon : MonoBehaviour
	{
		#region Fields

		public float fireInterval;
		public WeaponModifier modifier = WeaponModifier.Default;

		#endregion
		#region Methods

		public abstract void fire();

		#endregion
	}
}
