using UnityEngine;
using System.Collections;

namespace Game.Items
{ 
  public class Item : MonoBehaviour
  {
    #region Fields
    public int pointsValue = 10;
    public string type = "POINTS";
    #endregion

    #region Methods

    // Use this for initialization
    void Start()
    {
      // GetComponent<Rigidbody2D>().velocity = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
      // Despawn when fallen to bottom
      if (transform.position.y < -15) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      switch(type) 
      {
        case "POINTS": 
          StatemachineStateIngame.getStatemachine().addScore(this.pointsValue);
          break;
      }


      Destroy(gameObject);
    }
    #endregion
  }
}