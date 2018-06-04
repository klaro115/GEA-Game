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
      calcScreenspace(radius * 2);
      translateFlightCoords();
      lastCheckpointIndex = flightBehaviour.Length - 1;
      transform.position = flightBehaviour[0];
    }

    protected override void Update ()
    {
      if(!Statemachine.IsIngame) return;

      if (checkpointReached()) checkpointIndex++;
      if(isDead()) Destroy(this.gameObject);
      else move();
    }

    #endregion
  }
}
