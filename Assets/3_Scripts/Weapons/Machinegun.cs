using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public class Machinegun : Weapon
	{
		#region Fields

		private float lastShotTime = 0.0f;

		// TEST
		public Transform projectile = null;

		#endregion
		#region Methods

		public override void fire ()
		{
			if(Time.time < lastShotTime + fireInterval) return;

			lastShotTime = Time.time;

			// TEST
			Debug.Log("Pew (" + Time.time + ")");
			projectile.position = transform.position;
		}

		// TEST
		void Update()
		{
			projectile.position += Vector3.up * 5 * Time.deltaTime;
		}

		#endregion
	}
}
