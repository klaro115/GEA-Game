using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public struct Projectile
	{
		#region Fields

		public bool isAlive;
		public Vector3 position;
		public Vector3 velocity;
		public int damage;

		public Transform physicalBody;

		#endregion
		#region Properties

		public static Projectile Blank
		{
			get
			{
				Projectile p = new Projectile();

				p.isAlive = false;
				p.position = Vector3.zero;
				p.velocity = Vector3.up;
				p.damage = 1;

				p.physicalBody = null;

				return p;
			}
		}

		#endregion
	}
}
