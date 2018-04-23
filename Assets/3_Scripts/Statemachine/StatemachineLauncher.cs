using UnityEngine;
using System.Collections;

namespace Game
{
	[AddComponentMenu("Scripts/State Machine/Launcher")]
	public class StatemachineLauncher : MonoBehaviour
	{
		#region Methods

		void Start ()
		{
			Statemachine.initialize();
		}
		
		void Update ()
		{
			Statemachine.update();
		}

		#endregion
	}
}
