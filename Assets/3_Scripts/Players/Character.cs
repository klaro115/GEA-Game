using UnityEngine;
using System.Collections;

using Game.Weapons;

namespace Game
{
  // TODO: Implement parent class including damage handlers for player and enemy!
  public abstract class Character : MonoBehaviour, IDamageReceiver
  {
    #region Fields
    public Weapon[] mainWeapons;
    public Weapon[] secWeapons;
    public int hitpoints;
    public float baseSpeed;
    public float radius;
    public Vector2 screenSpace;

    // init AudioSource
    protected AudioSource audioSource = null;

    #endregion
    #region Methods

    private void Awake()
    {
      // Set AudioSource
      audioSource = GetComponent<AudioSource>();
    }

    protected abstract void Update();

    protected abstract void Start();

    protected Vector2 calcScreenspace(float offset)
    {
      float orthoSize = Camera.main.orthographicSize;
      screenSpace.y = orthoSize - radius + offset*2;
      screenSpace.x = orthoSize * Camera.main.aspect - radius + offset*2;
      return screenSpace;
    }
	  
    protected Quaternion alignToTarget(Vector3 myPos, Vector3 targetPos)
    {
      targetPos.z = 0f;
      targetPos.x -= myPos.x;
      targetPos.y -= myPos.y;
      float angleToTarget = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
      angleToTarget -= 90f;
      return Quaternion.Euler(new Vector3(0, 0, angleToTarget));
    }

    protected void fireMainWeapons()
    {
      if (this.mainWeapons != null)
      {
        foreach (Weapon weapon in this.mainWeapons)
        {
          weapon.fire();
        }
      }

    }

    protected void fireSecondaryWeapons()
    {
      if (this.secWeapons.Length != 0 || this.secWeapons != null)
      {
        foreach (Weapon weapon in this.secWeapons)
        {
          weapon.fire();
        }
      }
    }

    public virtual void applyDamage(int dmg)
	  {
		  this.hitpoints -= dmg;
	  }

    protected bool isDead()
    {
      return this.hitpoints <= 0;
    }
    #endregion
  }
}
