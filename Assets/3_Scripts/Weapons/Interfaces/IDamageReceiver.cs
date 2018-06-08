using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
	public interface IDamageReceiver
	{
		void applyDamage(int damageReceived);
	}
}
