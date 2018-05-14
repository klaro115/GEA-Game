using UnityEngine;
using System.Collections;

namespace Game.Weapons
{
	public enum WeaponModifierType
	{
		Default,

		// TODO: Add more modifiers later... (scatter, laser, etc.)
	}

	[System.Serializable]
	public struct WeaponModifier
	{
		#region Fields

		public WeaponModifierType type;
		public int level;

		#endregion
		#region Properties

		public static WeaponModifier Default
		{
			get { return new WeaponModifier() { type=WeaponModifierType.Default, level=0 }; }
		}

		#endregion
	}
}
