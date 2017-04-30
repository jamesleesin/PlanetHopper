using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
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
	public float baseFuelConsumptionSpeed;
	public float baseMaxFuel;
	public float baseTimeBetweenAttacks;
	public float baseAttackDamage;
	public float baseCargoSpace;
	
	// levels of upgrades
	public int hpUpgrade = 0;
	public int shieldUpgrade = 0;
	public int turnSpeedUpgrade = 0;
	public int accelerationUpgrade = 0;
	public int hullHealSpeedUpgrade = 0;
	public int shieldHealSpeedUpgrade = 0;
	public int fuelConsumptionSpeedUpgrade = 0;
	public int fuelUpgrade = 0;
	public int timeBetweenAttacksUpgrade = 0;
	public int attackDamageUpgrade = 0;
	public int cargoSpaceUpgrade = 0;
	
	// transform variables
	public float playerRot;
	public Rigidbody2D playerRB;
	private GameObject gameGUI;
	
	// resources
	public float hp;
	public float shields;
	public float money;
	public float fuel;
	public float scrap;
	
	// status effects
	private bool canLand = false;
	private string landingPlanetName = "";
	public float fame = 0;
	
	// the fire images
	GameObject fireBack;
	GameObject fireLeft;
	GameObject fireRight;
	
	// Use this for initialization
	void Start () {
		// disable the fire images
		fireBack = transform.GetChild(0).gameObject;
		fireLeft = transform.GetChild(1).gameObject;
		fireRight = transform.GetChild(2).gameObject;
		fireBack.GetComponent<SpriteRenderer>().enabled = false;
		fireLeft.GetComponent<SpriteRenderer>().enabled = false;
		fireRight.GetComponent<SpriteRenderer>().enabled = false;
		
		if (PlayerPrefs.GetInt("SavedGame") != 1){
			// defaults
			baseMaxHp = 100;
			baseMaxShields = 10;
			baseTurnSpeed = 2;
			baseAcceleration = 50;
			baseMaxFuel = 500f;

			baseTimeBetweenAttacks = 0.5f;
			baseAttackDamage = 15f;
			baseHullHealSpeed = 0f;
			baseShieldHealSpeed = 0f;
			baseFuelConsumptionSpeed = 10f;
						
			hp = baseMaxHp;
			shields = baseMaxShields;
			fuel = baseMaxFuel;
			money = 100f;
			scrap = 0f;
			transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
		}
		else{
			// loaded values
			baseMaxHp = PlayerPrefs.GetFloat("BaseMaxHP");
			baseMaxShields = PlayerPrefs.GetFloat("BaseMaxShields");
			baseTurnSpeed = PlayerPrefs.GetFloat("BaseTurnSpeed");
			baseAcceleration = PlayerPrefs.GetFloat("BaseAcceleration");
			baseMaxFuel = PlayerPrefs.GetFloat("BaseMaxFuel");
			baseFuelConsumptionSpeed = PlayerPrefs.GetFloat("BaseFuelConsumptionSpeed");
			baseAttackDamage = PlayerPrefs.GetFloat("BaseAttackDamage");
			baseTimeBetweenAttacks = PlayerPrefs.GetFloat("BaseTimeBetweenAttacks");
			baseHullHealSpeed = PlayerPrefs.GetFloat("BaseHullHealSpeed");
			baseShieldHealSpeed = PlayerPrefs.GetFloat("BaseShieldHealSpeed");
			baseCargoSpace = PlayerPrefs.GetFloat("BaseCargoSpace");
			
			hpUpgrade = PlayerPrefs.GetInt("HpUpgrade");
			shieldUpgrade = PlayerPrefs.GetInt("ShieldUpgrade");
			turnSpeedUpgrade = PlayerPrefs.GetInt("TurnSpeedUpgrade");
			accelerationUpgrade = PlayerPrefs.GetInt("AccelerationUpgrade");
			hullHealSpeedUpgrade = PlayerPrefs.GetInt("HullHealSpeedUpgrade");
			shieldHealSpeedUpgrade = PlayerPrefs.GetInt("ShieldHealSpeedUpgrade");
			fuelConsumptionSpeedUpgrade = PlayerPrefs.GetInt("FuelConsumptionSpeedUpgrade");
			fuelUpgrade = PlayerPrefs.GetInt("FuelUpgrade");
			timeBetweenAttacksUpgrade = PlayerPrefs.GetInt("TimeBetweenAttacksUpgrade");
			attackDamageUpgrade = PlayerPrefs.GetInt("AttackDamageUpgrade");
			cargoSpaceUpgrade = PlayerPrefs.GetInt("CargoSpaceUpgrade");
			
			hp = baseMaxHp + hpUpgrade*10f;
			shields = baseMaxShields + shieldUpgrade*5f;
			fuel = baseMaxFuel + fuelUpgrade*100f;
			money = PlayerPrefs.GetFloat("Money");
			scrap = PlayerPrefs.GetFloat("Scrap");
			
			// load game from a planet
			canLand = true;
			landingPlanetName = PlayerPrefs.GetString("CurrentPlanet");
			
			fame = PlayerPrefs.GetFloat("Fame");
			
			transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), -1f);
		}
		
		playerRB = GetComponent<Rigidbody2D>();
		playerRB.mass = 50f;
		gameGUI = GameObject.Find("GUI");
	}
	
	public void GainScrap(float amt){
		if (scrap + amt >= baseCargoSpace+cargoSpaceUpgrade*50f){
			scrap = baseCargoSpace+cargoSpaceUpgrade*50f;
		}
		else{
			scrap += amt;
		}
	}
	
	// death animation
	void Death(){
		
	}
	
	void SaveGame(){
		PlayerPrefs.SetInt("SavedGame", 1);
		PlayerPrefs.SetFloat("HP", hp);
		PlayerPrefs.SetFloat("Shields", shields);
		PlayerPrefs.SetFloat("Money", money);
		PlayerPrefs.SetFloat("Scrap", scrap);
		
		PlayerPrefs.SetFloat("BaseMaxHp", baseMaxHp);
		PlayerPrefs.SetFloat("BaseMaxShields", baseMaxShields);
		PlayerPrefs.SetFloat("BaseTurnSpeed", baseTurnSpeed);
		PlayerPrefs.SetFloat("BaseAcceleration", baseAcceleration);
		PlayerPrefs.SetFloat("BaseMaxFuel", baseMaxFuel);
		PlayerPrefs.SetFloat("BaseFuelConsumptionSpeed", baseFuelConsumptionSpeed);
		PlayerPrefs.SetFloat("BaseAttackDamage", baseAttackDamage);
		PlayerPrefs.SetFloat("BaseHullHealSpeed", baseHullHealSpeed);
		PlayerPrefs.SetFloat("BaseShieldHealSpeed", baseShieldHealSpeed);
		PlayerPrefs.SetFloat("BaseTimeBetweenAttacks", baseTimeBetweenAttacks);
		PlayerPrefs.SetFloat("BaseCargoSpace", baseCargoSpace);
		
		PlayerPrefs.SetFloat("PlayerX", transform.position.x);
		PlayerPrefs.SetFloat("PlayerY", transform.position.y);
		PlayerPrefs.SetString("CurrentPlanet", landingPlanetName);
		PlayerPrefs.SetFloat("Fame", fame);
	}
	
	void LandOnPlanet(){
		// land on the planet, restoring hp, shields, fuel
		hp = baseMaxHp + hpUpgrade*10f;
		shields = baseMaxShields + shieldUpgrade*5f;
		fuel = baseMaxFuel + fuelUpgrade*100f;
	
		// save everything into preferences
		SaveGame();
		// then open the planet scene
		SceneManager.LoadScene("Planet", LoadSceneMode.Single);
	}
	
	void Shoot(){
		// if attack available, then shoot 1 attack from each gun
		if (attackCooldown <= 0){
			attackCooldown = baseTimeBetweenAttacks*Mathf.Pow(0.9f, timeBetweenAttacksUpgrade);
			// left attack
			Attack atkLeft = Instantiate(attackPrefab) as Attack;
			Vector3 leftPosition = transform.GetChild(3).gameObject.transform.position;
			atkLeft.Initialize(leftPosition.x, leftPosition.y, playerRB.velocity + (new Vector2(transform.up.x, transform.up.y)*5f), baseAttackDamage + attackDamageUpgrade*5);
			attackList.Add(atkLeft);
			// right attack
			Attack atkRight = Instantiate(attackPrefab) as Attack;
			Vector3 rightPosition = transform.GetChild(4).gameObject.transform.position;
			atkRight.Initialize(rightPosition.x, rightPosition.y, playerRB.velocity + (new Vector2(transform.up.x, transform.up.y)*5f), baseAttackDamage + attackDamageUpgrade*5);
			attackList.Add(atkRight);
		}
	}
	
	void UpdateClosestPlanet(){
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		Planet closestPlanet = gm.planetList[0];
		float closestDistance = Vector3.Distance(transform.position, closestPlanet.transform.position);
		for (int i = 1; i < gm.planetList.Count; i++){
			if (Vector3.Distance(transform.position, gm.planetList[i].transform.position) < closestDistance){
				closestPlanet = gm.planetList[i];
				closestDistance = Vector3.Distance(transform.position, gm.planetList[i].transform.position);
			}
		}
		string directions = "";
		if (closestPlanet.transform.position.y < transform.position.y){
			directions += "South: " + (transform.position.y -closestPlanet.transform.position.y).ToString("F1");
		}
		else {
			directions += "North: " + (closestPlanet.transform.position.y - transform.position.y).ToString("F1");
		}
		if (closestPlanet.transform.position.x < transform.position.x){
			directions += " West: " + (transform.position.x -closestPlanet.transform.position.x).ToString("F1");
		}
		else {
			directions += " East: " + (closestPlanet.transform.position.x - transform.position.x).ToString("F1");
		}
		
		
		gameGUI.transform.Find("ClosestPlanet").GetComponent<Text>().text = "Closest Planet: " + closestPlanet.planetName + "\n" + directions;
	}
	
	void RotateLeft () {
		transform.Rotate (Vector3.forward * (baseTurnSpeed+turnSpeedUpgrade));
	}
	void RotateRight () {
		transform.Rotate (Vector3.forward * -1f * (baseTurnSpeed+turnSpeedUpgrade));
	}
	
	// when player collides with something
    void OnTriggerStay2D(Collider2D obj)
    {
		if (obj.tag == "Asteroid")
        {
			hp -= 5f * Time.deltaTime;
		}
	}
	
	void OnTriggerEnter2D(Collider2D obj){
		if (obj.tag == "Planet"){
			canLand = true;
			Planet landingPlanet = obj.GetComponent<Planet>();
			landingPlanetName = landingPlanet.planetName;
		}
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
	
	void OnTriggerExit2D(Collider2D obj){
		if (obj.tag == "Planet"){
			canLand = false;
			landingPlanetName = "";
		}
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -10f);
		if (Input.GetKey(KeyCode.LeftArrow)){
			RotateLeft();
			fireRight.GetComponent<SpriteRenderer>().enabled = true;
		}
		if (Input.GetKey(KeyCode.RightArrow)){
			RotateRight();
			fireLeft.GetComponent<SpriteRenderer>().enabled = true;
		}
		// disable images on release
		if (Input.GetKeyUp(KeyCode.LeftArrow)){
			fireRight.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow)){
			fireLeft.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (Input.GetKeyUp(KeyCode.UpArrow)){
			fireBack.GetComponent<SpriteRenderer>().enabled = false;
		}
		
		if (fuel > 0){
			if (Input.GetKey(KeyCode.UpArrow)){
				fuel -= (baseFuelConsumptionSpeed * Mathf.Pow(0.9f, fuelConsumptionSpeedUpgrade)) * Time.deltaTime;
				fireBack.GetComponent<SpriteRenderer>().enabled = true;
				playerRB.AddForce(transform.up * (baseAcceleration + accelerationUpgrade * 10f));
			}
		}
		
		// player tries to land
		if (Input.GetKey(KeyCode.Space)){
			if (canLand){
				LandOnPlanet();
			}
		}
		
		// shoot
		if (Input.GetKey(KeyCode.A)){
			Shoot();
		}
		// attack cooldown
		if (attackCooldown > 0){
			attackCooldown -= Time.deltaTime;
		}
		
		// scrap heals over time
		if (hp < baseMaxHp+hpUpgrade*10f){
			hp += (baseHullHealSpeed+hullHealSpeedUpgrade) * Time.deltaTime;
		}
		if (shields < baseMaxShields+shieldUpgrade*5f)
			shields += (baseShieldHealSpeed+shieldHealSpeedUpgrade) * Time.deltaTime;
		
		// update GUI
		gameGUI.transform.Find("Hull").transform.Find("Text").GetComponent<Text>().text = (hp*100f/(baseMaxHp+hpUpgrade*10f)).ToString("F1") + "%";
		gameGUI.transform.Find("Shields").transform.Find("Text").GetComponent<Text>().text = (shields*100f/(baseMaxShields+shieldUpgrade*5f)).ToString("F1") + "%";
		gameGUI.transform.Find("Fuel").transform.Find("Text").GetComponent<Text>().text = (fuel*100f/(baseMaxFuel+fuelUpgrade)).ToString("F1") + "%";
		gameGUI.transform.Find("Money").transform.Find("Text").GetComponent<Text>().text = money.ToString("F1");
		gameGUI.transform.Find("Scrap").transform.Find("Text").GetComponent<Text>().text = scrap.ToString("F1");
		
		// update closest planet
		UpdateClosestPlanet();
		
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
