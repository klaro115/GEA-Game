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
      Player player = StatemachineStateIngame.getStatemachine().Player;
      switch (type) 
      {
        case "POINTS":
          StatemachineStateIngame.getStatemachine().addScore(this.pointsValue);
          break;
        case "HITPOINTS_PLUS":
          player.incHitpoints();
          break;
        case "MOD_CROSS_PLUS":
          player.incModCrossLevel();
          break;
        case "MOD_SCATTER_PLUS":
          player.incModScatterLevel();
          break;
        case "TYPE_MG":
          player.setWeaponType(type);
          break;
        case "TYPE_LASER":
          player.setWeaponType(type);
          break;
        default: 
          Debug.Log("Item.type " + type + " unknown.");
          break;
      }
      Destroy(gameObject);
    }
    #endregion
  }
}