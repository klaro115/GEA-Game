using UnityEngine;
using System.Collections;
using Game.Weapons;

namespace Game
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Player : Character
	{
		#region Fields
		public Vector2 screenSpace;

		private Rigidbody2D rig;

		#endregion
		#region Methods

		protected override void Start ()
		{
			rig = GetComponent<Rigidbody2D>();

			//mainWeapon = null;
			//secWeapon = null;
			hitpoints = 100;
			baseSpeed = 10.0f;
			radius = 0.5f;

			// Define Screenspace
			float orthoSize = Camera.main.orthographicSize;
			screenSpace.y = orthoSize - radius;
			screenSpace.x = orthoSize * Camera.main.aspect - radius;
		}

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;

			// Fetch input signals:
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			// Move the ship:
			//transform.Translate(new Vector3(x, y, 0) * baseSpeed * Time.deltaTime);
			rig.velocity = new Vector3(x, y, 0) * baseSpeed;

			// Limit position to screen space:
			Vector3 pos = transform.position;
			pos.x = Mathf.Clamp(pos.x, -screenSpace.x, screenSpace.x);
			pos.y = Mathf.Clamp(pos.y, -screenSpace.y, screenSpace.y);
			transform.position = pos;

			if(mainWeapon != null && Input.GetKey(KeyCode.Space))
			{
				mainWeapon.fire();
			}
		}

		#endregion
	}
}
