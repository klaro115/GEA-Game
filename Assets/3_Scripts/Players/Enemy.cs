using UnityEngine;
using System.Collections;

namespace Game
{
	public class Enemy : MonoBehaviour
	{
		#region Fields

		#endregion
		#region Methods

		void Start ()
		{

		}

		void Update ()
		{
			if(!Statemachine.IsIngame) return;

		}

		#endregion
	}
}
