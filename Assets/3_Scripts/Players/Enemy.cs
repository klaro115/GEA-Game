using UnityEngine;
using System.Collections;

namespace Game
{
	public class Enemy : Character
	{
		#region Fields

		#endregion
		#region Methods

		protected override void Start ()
		{

		}

		protected override void Update ()
		{
			if(!Statemachine.IsIngame) return;

		}

		#endregion
	}
}
