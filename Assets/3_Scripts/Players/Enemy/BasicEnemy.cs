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
      // Use Update() Method of superclass
      base.Update();
    }

    #endregion
  }
}
