using System.Collections;
using UnityEngine;

namespace Game.Weapons
{
	public class ProjectileHandlerTester : MonoBehaviour
	{
		#region Methods
		
		void Start ()
		{
			ProjectileHandler.initialize();
		}
		
		void Update ()
		{
			ProjectileHandler.update();
		}

		#endregion
	}
}
