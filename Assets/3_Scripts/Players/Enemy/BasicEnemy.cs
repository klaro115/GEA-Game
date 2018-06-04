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
      transform.position = flightBehaviour[0];
    }

    protected override void Update ()
    {
      if(!Statemachine.IsIngame) return;
      float distanceOffset = radius;
      float distanceToNextCheckpoint = ((Vector2)transform.position - flightBehaviour[checkpointIndex + 1]).magnitude;
      int lastCheckpointIndex = flightBehaviour.Length - 1;
      if(distanceToNextCheckpoint < distanceOffset) checkpointIndex++;
      if(checkpointIndex >= lastCheckpointIndex) Destroy(this.gameObject);
      else {
        Vector2 flightDirection = (flightBehaviour[checkpointIndex + 1] - flightBehaviour[checkpointIndex]).normalized;
        transform.position += baseSpeed * (Vector3)flightDirection * Time.deltaTime;
      }
    }

    #endregion
  }
}
