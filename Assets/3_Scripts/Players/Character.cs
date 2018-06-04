using UnityEngine;
using System.Collections;

using Game.Weapons;

namespace Game
{
  // TODO: Implement parent class including damage handlers for player and enemy!
  public abstract class Character : MonoBehaviour
  {
    #region Fields
    public Weapon mainWeapon;
    public Weapon secWeapon;
    public int hitpoints;
    public float baseSpeed;
    public float radius;

	protected Rigidbody2D rig;

    #endregion
    #region Methods

    protected abstract void Start();
    protected abstract void Update();

    #endregion
  }
}
