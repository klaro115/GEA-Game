using UnityEngine;
using System.Collections;

using Game.Weapons;

namespace Game
{
	// TODO: Implement parent class including damage handlers for player and enemy!
	public class Player : MonoBehaviour
	{
		#region Fields

		public float speed = 2.0f;
		public Vector2 screenSpace = new Vector2(10, 10);
		public float radius = 0.5f;

		public Weapon mainWeapon = null;
		public Weapon secWeapon = null;

		#endregion
		#region Methods

		void Start ()
		{
			float orthoSize = Camera.main.orthographicSize;
			screenSpace.y = orthoSize - radius;
			screenSpace.x = orthoSize * Camera.main.aspect - radius;
		}
		
		void Update ()
		{
			if(!Statemachine.IsIngame) return;

			// Fetch input signals:
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			// Move the ship:
			transform.Translate(new Vector3(x, y, 0) * speed * Time.deltaTime);

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
