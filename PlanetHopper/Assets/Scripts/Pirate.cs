using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour {
	public int pirateType = 1;
	
	public Attack attackPrefab;
	public List<Attack> attackList = new List<Attack>();
	public float attackCooldown = 0;
	// cleanup
	public float cleanupTimer = 0f;
	
	public float baseMaxHp;
	public float baseMaxShields;
	public float baseTurnSpeed;
	public float baseAcceleration;
	public float baseHullHealSpeed;
	public float baseShieldHealSpeed;
	public float baseTimeBetweenAttacks;
	public float baseAttackDamage;
	
	// transform variables
	public float pirateRot;
	public Rigidbody2D pirateRB;
	GameObject targetPlayer;
	
	// resources
	public float hp;
	public float shields;
	
	// the fire images
	GameObject fireBack;
	GameObject fireLeft;
	GameObject fireRight;
	
	// Use this for initialization
	void Awake () {
		// disable the fire images
		fireBack = transform.GetChild(0).gameObject;
		fireLeft = transform.GetChild(1).gameObject;
		fireRight = transform.GetChild(2).gameObject;
		fireBack.GetComponent<SpriteRenderer>().enabled = false;
		fireLeft.GetComponent<SpriteRenderer>().enabled = false;
		fireRight.GetComponent<SpriteRenderer>().enabled = false;
		pirateRB = GetComponent<Rigidbody2D>();

		// defaults
		// small ship, fast but little damage
		if (pirateType == 1){
			baseMaxHp = 100;
			baseMaxShields = 50;
			baseTurnSpeed = 6;
			baseAcceleration = 100;

			baseTimeBetweenAttacks = 0.3f;
			baseAttackDamage = 5f;
			baseHullHealSpeed = 0f;
			baseShieldHealSpeed = 1f;	
			pirateRB.mass = 50f;
		}
		// medium ship, average speed but tankier
		else if (pirateType == 2){
			baseMaxHp = 250;
			baseMaxShields = 150;
			baseTurnSpeed = 4;
			baseAcceleration = 2;

			baseTimeBetweenAttacks = 0.5f;
			baseAttackDamage = 15f;
			baseHullHealSpeed = 1f;
			baseShieldHealSpeed = 1f;	
		}
		// battleship, run away!
		else if (pirateType == 3){
			baseMaxHp = 600;
			baseMaxShields = 350;
			baseTurnSpeed = 3;
			baseAcceleration = 1;

			baseTimeBetweenAttacks = 0.35f;
			baseAttackDamage = 50f;
			baseHullHealSpeed = 5f;
			baseShieldHealSpeed = 5f;	
		}
		
		
		hp = baseMaxHp;
		shields = baseMaxShields;

		transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
	
		targetPlayer = GameObject.Find("Player(Clone)");
	}
	
	void Death(){
		GameObject.Find("Player(Clone)").GetComponent<Player>().GainScrap(Random.Range(50f, 150f)*pirateType);
		GameObject.Find("Player(Clone)").GetComponent<Player>().fame += pirateType/2f;
	}
	
	void Shoot(){
		// if attack available, then shoot 1 attack from each gun
		if (attackCooldown <= 0){
			attackCooldown = baseTimeBetweenAttacks;
			// left attack
			Attack atkLeft = Instantiate(attackPrefab) as Attack;
			Vector3 leftPosition = transform.GetChild(3).gameObject.transform.position;
			atkLeft.Initialize(leftPosition.x, leftPosition.y, pirateRB.velocity + (new Vector2(transform.up.x, transform.up.y)*5f), baseAttackDamage);
			attackList.Add(atkLeft);
			// right attack
			Attack atkRight = Instantiate(attackPrefab) as Attack;
			Vector3 rightPosition = transform.GetChild(4).gameObject.transform.position;
			atkRight.Initialize(rightPosition.x, rightPosition.y, pirateRB.velocity + (new Vector2(transform.up.x, transform.up.y)*5f), baseAttackDamage);
			attackList.Add(atkRight);
		}
	}
	
	void RotateLeft () {
		transform.Rotate (Vector3.forward * baseTurnSpeed);
	}
	void RotateRight () {
		transform.Rotate (Vector3.forward * -1f * baseTurnSpeed);
	}
	
	// when pirate collides with something
    void OnTriggerStay2D(Collider2D obj)
    {
	}
	
	// hit by attacks
	void OnTriggerEnter2D(Collider2D obj){
		if (obj.tag == "Attack"){
			Attack atk = obj.GetComponent<Attack>();
			if (!attackList.Contains(atk)){
				hp -= atk.damage;
				if (hp <= 0){
					Death();
				}
				Destroy(obj.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// calculate player pos, and add aiming helping vectors
		Vector3 playerPos = targetPlayer.transform.position + (new Vector3(targetPlayer.GetComponent<Rigidbody2D>().velocity.x - pirateRB.velocity.x, targetPlayer.GetComponent<Rigidbody2D>().velocity.y - pirateRB.velocity.y, 0f));
		float angle = 0;
		// calculate which direction to rotate to
		if (playerPos.x != transform.position.x && playerPos.y != transform.position.y){
			float yDiff;
			float xDiff;
			if (transform.position.y < playerPos.y){
				yDiff = playerPos.y - transform.position.y;
				if (transform.position.x < playerPos.x){
					xDiff = playerPos.x - transform.position.x;
					angle = -1f*(90f-  Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg);
				}
				else{
					xDiff = transform.position.x - playerPos.x;
					angle = 90f- (Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg);
				}
			}
			else{
				yDiff = transform.position.y - playerPos.y;	
				if (transform.position.x < playerPos.x){
					xDiff = playerPos.x - transform.position.x;
					angle = -1f*(90f+ Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg);
				}
				else{
					xDiff = transform.position.x - playerPos.x;
					angle = 90f+ (Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg);
				}
			}
		}
		float currentRot = (transform.eulerAngles.z > 180) ? -360f + transform.eulerAngles.z : transform.eulerAngles.z;
		if (currentRot < angle){
			RotateLeft();
			fireRight.GetComponent<SpriteRenderer>().enabled = true;
			fireLeft.GetComponent<SpriteRenderer>().enabled = false;
		}
		else if (currentRot > angle){
			RotateRight();
			fireLeft.GetComponent<SpriteRenderer>().enabled = true;
			fireRight.GetComponent<SpriteRenderer>().enabled = false;
		}

		Debug.Log(currentRot);
		if (Vector3.Distance(playerPos, transform.position) > 5f){
			fireBack.GetComponent<SpriteRenderer>().enabled = true;
			pirateRB.AddForce(transform.up * baseAcceleration);
		}
		else{
			fireBack.GetComponent<SpriteRenderer>().enabled = false;
		}
		
		// attack cooldown
		if (attackCooldown > 0){
			attackCooldown -= Time.deltaTime;
		}
		else if (Vector3.Distance(playerPos, transform.position) < 15f){
			Shoot();
		}
		
		// heal over time
		if (hp < baseMaxHp){
			hp += baseHullHealSpeed * Time.deltaTime;
		}
		if (shields < baseMaxShields)
			shields += baseShieldHealSpeed * Time.deltaTime;
		
		// cleanup the attack list every 5 seconds
		if (cleanupTimer <= 0){
			cleanupTimer = 5f;
			for(int i = attackList.Count - 1; i > -1; i--)
			{
				if (attackList[i] == null)
				   attackList.RemoveAt(i);
			}
		}
		else{
			cleanupTimer -= Time.deltaTime;
		}
	}
}
