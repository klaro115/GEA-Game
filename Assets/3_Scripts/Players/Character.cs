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
    public Vector2 screenSpace;

    #endregion
    #region Methods

    protected abstract void Update();

    protected abstract void Start();

    protected Vector2 calcScreenspace(float offset)
    {
      float orthoSize = Camera.main.orthographicSize;
      screenSpace.y = orthoSize - radius + offset*2;
      screenSpace.x = orthoSize * Camera.main.aspect - radius + offset*2;
      return screenSpace;
    }
    
    // When Projectile.cs hits a Character, the given damage is applied
    void applyDamage(int dmg)
    {
		  this.hitpoints -= dmg;
      Debug.Log(dmg + "Damage applied. " + this.hitpoints + "left");
    }

    #endregion
  }
}
