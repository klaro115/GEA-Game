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
        case "HITPOINTS_PLUS":           
          StatemachineStateIngame.getStatemachine().Player.incHitpoints();
          break;
        case "MOD_CROSS_PLUS": 
          StatemachineStateIngame.getStatemachine().Player.incModCrossLevel();
          break;
        case "MOD_SCATTER_PLUS":
          StatemachineStateIngame.getStatemachine().Player.incModScatterLevel();
          break;
        case "TYPE_MG":
          break;
        case "TYPE_LASER":
          break;
        default: 
          Debug.Log("Item.type" + type + " unknown.");
          break;
      }
      Destroy(gameObject);
    }
    #endregion
  }
}