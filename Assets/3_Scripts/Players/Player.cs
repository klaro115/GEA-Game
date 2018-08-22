using UnityEngine;
using System.Collections;
using Game.Weapons;
using System;

namespace Game
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Player : Character
	{
		#region Fields

		private Rigidbody2D rig;

		public float damageCooldown = 0.1f;	// Minimum time span where damage is ignored after taking a hit.
		private float lastDamageReceivedTime = 0.0f;	// Time when the player last took damage.

    // init AudioClips
    private AudioClip soundCoinPickup = null;
    private AudioClip soundWeaponModPickup = null;
    private AudioClip soundWeaponTypePickup = null;
    private AudioClip soundHpPickup = null;
    private AudioClip soundPlayerHit = null;

		public static bool xOneActive = true;

    private static readonly string weaponPrefabMachinegun = "Machinegun";
		private static readonly string weaponPrefabLaser = "Lasergun";
    #endregion
    #region Methods
    
    protected override void Start ()
    {
      rig = GetComponent<Rigidbody2D>();
      // Set AudioClips
      soundCoinPickup = Resources.Load<AudioClip>("coin-pickup");
      soundWeaponModPickup = Resources.Load<AudioClip>("weapon-mod-pickup");
      soundWeaponTypePickup = Resources.Load<AudioClip>("weapon-type-pickup");
      soundHpPickup = Resources.Load<AudioClip>("hp-pickup");
      soundPlayerHit = Resources.Load<AudioClip>("player-hit");

      // Apply scale based on radius
      Vector3 size = new Vector3(radius * 2, radius * 2, radius * 2);
      this.transform.localScale = size;

			//mainWeapon = null;
			//secWeapon = null;
      calcScreenspace(0f);
    }

		protected override void Update ()
		{
			// Process input events for statemachine and game state logic:
			updateStateMachineInput();

			// Don't update game logic while game is paused or interrupted:
			if(!Statemachine.IsIngame)
			{
				// Cancel out velocity (so the player doesn't go flying off):
				rig.velocity = Vector2.zero;
				return;
			}

      // Check if dead
      // TODO: Show GameOver Screen and stop game
      // if (isDead()) Destroy(this.gameObject);  // NOTE: commented this out, for testing purposes.
			if(isDead())
			{
				// Switch ingame state to game over:
				StatemachineStateIngame.getStatemachine().setState(IngameState.GameOver);
				// Deactivate the player: (don't destroy as that may lead to null reference exceptions)
				gameObject.SetActive(false);
				return;
			}

			// Fetch input signals:
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			// Move the ship:
			//transform.Translate(new Vector3(x, y, 0) * baseSpeed * Time.deltaTime);
			rig.velocity = new Vector3(x, y, 0) * baseSpeed;

			// Limit position to screen space:
			Vector3 pos = transform.position;
			pos.x = Mathf.Clamp(pos.x, -screenSpace.x, screenSpace.x);
			pos.y = Mathf.Clamp(pos.y, -screenSpace.y, screenSpace.y);
			transform.position = pos;

			bool x360Input = !xOneActive && (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.JoystickButton16));
			bool xOneInput = xOneActive && Input.GetKey(KeyCode.JoystickButton1);
			bool x360Input2 = !xOneActive && (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button17));
			bool xOneInput2 = xOneActive && Input.GetKey(KeyCode.JoystickButton2);

			if(Input.GetKey(KeyCode.Space) || x360Input || xOneInput)
			{
				fireMainWeapons();
			}
			if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Return) || x360Input2 || xOneInput2)

      {
				fireSecondaryWeapons();
			}

			//CHEAT:
			if(Input.GetKeyDown(KeyCode.O))
			{
				hitpoints++;
			}
		}

		private void updateStateMachineInput ()
		{
			StatemachineStateIngame ingameState = StatemachineStateIngame.getStatemachine();

			// Press 'escape' to pause or unpause the game:
			if(Input.GetKeyDown(KeyCode.Escape) ||
				(xOneActive && Input.GetKeyDown(KeyCode.JoystickButton12)) ||
				(!xOneActive && Input.GetKeyDown(KeyCode.JoystickButton9) || Input.GetKeyDown(KeyCode.JoystickButton7)))
			{
				if(ingameState.IngameState == IngameState.Ingame)
					ingameState.setState(IngameState.Paused);
				else if(ingameState.IngameState == IngameState.Paused)
					ingameState.setState(IngameState.Ingame);
			}
		}
	
	  	public override void applyDamage(int dmg)
		{
			// Only allow player to repeatedly take damage after a cooldown period has passed:
			if(Time.time > damageCooldown + lastDamageReceivedTime)
			{
        audioSource.PlayOneShot(this.soundPlayerHit);
        lastDamageReceivedTime = Time.time;
				// Debug.Log("Player Hit! " + this.hitpoints + " left.");
				this.hitpoints -= dmg;
			}
		}

    public void addScore(int value)
    {
      audioSource.PlayOneShot(this.soundCoinPickup);
      StatemachineStateIngame.getStatemachine().addScore(value);
    }
		
		public void incHitpoints()
		{
      audioSource.PlayOneShot(soundHpPickup);
      hitpoints += 1;
		}

		public void incModCrossLevel()
		{
      audioSource.PlayOneShot(soundWeaponModPickup);
      if (mainWeapons[0].modifier.type == Weapons.WeaponModifierType.CrossShot)
			{
				mainWeapons[0].modifier.level += 1;
			}else
			{
				mainWeapons[0].modifier.type = Weapons.WeaponModifierType.CrossShot;
				mainWeapons[0].modifier.level = 1;
			}
		}

		public void incModScatterLevel()
		{
      audioSource.PlayOneShot(soundWeaponModPickup);
      if (mainWeapons[0].modifier.type == Weapons.WeaponModifierType.ScatterShot)
			{
				mainWeapons[0].modifier.level += 1;
			}else
			{
				mainWeapons[0].modifier.type = Weapons.WeaponModifierType.ScatterShot;
				mainWeapons[0].modifier.level = 1;
			}
		}

    // Set the players main weapon type to the given type
    public void setWeaponType(string type)
    {
      Weapon currentWeapon = mainWeapons[0];
			Weapon newWeaponPrefab = null;
      // Switch over possible Weapon types
      switch (type)
      {
        case "TYPE_MG":
          		// Switch weapon type to MG:
				  {
					  Debug.Log("Switching to MG");
					  if(currentWeapon.GetType() == typeof(Machinegun)) break;
					  newWeaponPrefab = Resources.Load<Weapon>(weaponPrefabMachinegun);
            // set fireInterval to players fire interval
            newWeaponPrefab.fireInterval = 0.3f;
            ((Machinegun)newWeaponPrefab).damage = 10;
          }
          break;
        case "TYPE_LASER":
				// Switch weapon type to Laser:
				{
					Debug.Log("Switching to Laser");
					if(currentWeapon.GetType() == typeof(Lasergun)) break;
					newWeaponPrefab = Resources.Load<Weapon>(weaponPrefabLaser);
          }
          break;
        default:
          Debug.Log("Weapon.type " + type + " unknown.");
          break;
      }

      audioSource.PlayOneShot(soundWeaponTypePickup);
      if (newWeaponPrefab != null)
			{
        // Spawn new weapon in the same location and rotation as current weapon:
        Transform curWepTrans = currentWeapon.transform;
				GameObject newWeaponGO = Instantiate(newWeaponPrefab.gameObject,
					curWepTrans.position, curWepTrans.rotation, curWepTrans.parent) as GameObject;
				Weapon newWeapon = newWeaponGO.GetComponent<Weapon>();
				newWeapon.playerControlled = true;

				// Assign new weapon and discard previous one:
				if(newWeapon != null)
				{
					newWeapon.modifier = currentWeapon.modifier;
					Destroy(currentWeapon.gameObject);
					mainWeapons[0] = newWeapon;
				}
			}
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			// Define some constants for dealing collision damage:
			const int targetCollisionDamage = 10;
			const int playerCollisionDamage = 1;

			// Get the other party we collided with:
			GameObject target = collision.gameObject;
			// Don't apply collision damage between allied characters:
			if(target.layer == gameObject.layer)
			{
				return;
				// NOTE: This may be needed in later stages, when dealing with other players or AI companions.
			}

			// Apply damage to collision target:
			target.SendMessage("applyDamage", targetCollisionDamage, SendMessageOptions.DontRequireReceiver);

			// Apply collision damage to self:
			applyDamage(playerCollisionDamage);
		}

		#endregion
	}
}
