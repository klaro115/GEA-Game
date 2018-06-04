using UnityEngine;
using System.Collections;

namespace Game
{
  public class BasicEnemy : Enemy
  {
    #region Fields

    #endregion
    #region Methods

    protected override void Start ()
    {
      hitpoints = 100;
      baseSpeed = 5.0f;
      radius = 0.5f;

      calcScreenspace(radius * 2);
      translateFlightCoords();
      transform.position = flightBehaviour[0];
    }

    protected override void Update ()
    {
      if(!Statemachine.IsIngame) return;

      Vector2 flightDirection = (flightBehaviour[1] - (Vector2)transform.position).normalized;
      transform.position += baseSpeed * (Vector3)flightDirection * Time.deltaTime;
    }

    #endregion
  }
}
