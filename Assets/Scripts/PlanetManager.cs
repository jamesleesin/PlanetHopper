using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class PlanetManager : MonoBehaviour {
	// stats
	public float hp;
	public float shields;
	public float fuel;
	public float money;
	public float scrap;
	public float maxhp;
	public float maxshields;
	public float maxfuel;
	
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
	
	private GameObject gameGUI;
	public Sprite[] landscapeImages;
	
	public string planetName;
	public List<string> greetings = new List<string>();
	public List<string> upgrades = new List<string>();
	public List<string> availableUpgrades = new List<string>();
	
	public float currentBuyPrice = 0f;
	public float playerFame;
	
	// populate GUI with player stats and greetings
	void Awake(){
		gameGUI = GameObject.Find("Canvas");

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
		
		money = PlayerPrefs.GetFloat("Money");
		scrap = PlayerPrefs.GetFloat("Scrap");
		planetName = PlayerPrefs.GetString("CurrentPlanet");
		playerFame = PlayerPrefs.GetFloat("Fame");
			
		maxhp = PlayerPrefs.GetFloat("BaseMaxHp") + hpUpgrade*10f;
		maxshields = PlayerPrefs.GetFloat("MaxShields") + shieldUpgrade*5f;
		maxfuel = PlayerPrefs.GetFloat("MaxFuel") + fuelUpgrade*100f;;
		
		hp = maxhp;
		shields = maxshields;
		fuel = maxfuel;
		
		// greetings based on fame
		greetings.Add("Who the hell are you? Get out of here before I - what? You're here to trade? Well then, why didn't you say so? Make it quick. " + planetName + " isn't welcoming to outsiders.");
		greetings.Add("Oh. It's you again. Welcome back to " + planetName + "... I suppose you can unload your wares over there.");
		greetings.Add("Hey, welcome back to " + planetName + ". Damn, rough day out there? Your ship looks like it's seen better days.");
		greetings.Add("How are the conditions out there? Be careful, it's getting more and more dangerous lately, and it'd be a shame if we lost a great supplier such as yourself.");
		greetings.Add("Good to see you again! Come back soon, " + planetName + " always has its doors open for you.");
		
		// upgrades available random on each visit
		upgrades.Add("Hull Fortification: +10 Max Hull");
		upgrades.Add("Shield Strength: +5 Max Shield");
		upgrades.Add("Enhance Thrusters: +1 Turn Speed");
		upgrades.Add("Enhance Rockets: +10 Acceleration");
		upgrades.Add("Widen Fuel Tank: +100 Max Fuel");
		upgrades.Add("Fuel Efficiency: -10% Fuel Consumption");
		upgrades.Add("Enhance Firepower: +5 Damage");
		upgrades.Add("Nanobot Upgrade: +0.3 Hull Heal Speed");
		upgrades.Add("Enhance Battery: +0.5 Shield Heal Speed");
		upgrades.Add("Improved Reload: +10% Attack Speed");	
		upgrades.Add("Increased Cargo Space: +50 Cargo Space");	
		
		// the currently available upgrades
		for (int i = 0; i < 4; i++){
			int index = Random.Range(0, upgrades.Count);
			availableUpgrades.Add(upgrades[index]);
			upgrades.RemoveAt(index);
		}
		
		// set scrap buy price
		currentBuyPrice = Random.Range(10f, 20f);
		gameGUI.transform.Find("Sell").GetComponent<Text>().text = "Sell Scrap (Price: " + (currentBuyPrice + playerFame).ToString("F1") + " ea.)";
		
		if (playerFame <= 5f){
			gameGUI.transform.Find("GreetingText").GetComponent<Text>().text = greetings[0];
		}
		else {
			gameGUI.transform.Find("GreetingText").GetComponent<Text>().text = greetings[1];
		}
	
		
		// planet landscape image based on planet names
		if (planetName == "Earth"){
			gameGUI.transform.Find("Picture").GetComponent<Image>().sprite = landscapeImages[0];
		}
		else if (planetName == "Mars"){
			gameGUI.transform.Find("Picture").GetComponent<Image>().sprite = landscapeImages[1];
		}
		else if (planetName == "Galio"){
			gameGUI.transform.Find("Picture").GetComponent<Image>().sprite = landscapeImages[2];
		}
		else{
			gameGUI.transform.Find("Picture").GetComponent<Image>().sprite = landscapeImages[3];
		}
		// adjust the image height and width to match
		if (planetName == "Earth"){
			gameGUI.transform.Find("Picture").transform.localScale = new Vector3(1f, 0.66f, 1f);
		}
		else{
			gameGUI.transform.Find("Picture").transform.localScale = new Vector3(0.66f, 1f, 1f);
		}
	}
	
	public void SaveUpgrades(){
		PlayerPrefs.SetInt("SavedGame", 1);
		PlayerPrefs.SetFloat("Money", money);
		PlayerPrefs.SetFloat("Fame", playerFame);
		
		PlayerPrefs.SetInt("HpUpgrade", hpUpgrade);
		PlayerPrefs.SetInt("ShieldUpgrade", shieldUpgrade);
		PlayerPrefs.SetInt("TurnSpeedUpgrade", turnSpeedUpgrade);
		PlayerPrefs.SetInt("AccelerationUpgrade", accelerationUpgrade);
		PlayerPrefs.SetInt("FuelUpgrade", fuelUpgrade);
		PlayerPrefs.SetInt("FuelConsumptionSpeedUpgrade", fuelConsumptionSpeedUpgrade);
		PlayerPrefs.SetInt("AttackDamageUpgrade", attackDamageUpgrade);
		PlayerPrefs.SetInt("HullHealSpeedUpgrade", hullHealSpeedUpgrade);
		PlayerPrefs.SetInt("ShieldHealSpeedUpgrade", shieldHealSpeedUpgrade);
		PlayerPrefs.SetInt("TimeBetweenAttacksUpgrade", timeBetweenAttacksUpgrade);
		PlayerPrefs.SetInt("CargoSpaceUpgrade", cargoSpaceUpgrade);
	}

	void Update(){
		// update GUI
		gameGUI.transform.Find("Hull").transform.Find("Text").GetComponent<Text>().text = (hp*100f/maxhp).ToString("F1") + "%";
		gameGUI.transform.Find("Shields").transform.Find("Text").GetComponent<Text>().text = (shields*100f/maxshields).ToString("F1") + "%";
		gameGUI.transform.Find("Fuel").transform.Find("Text").GetComponent<Text>().text = (fuel*100f/maxfuel).ToString("F1") + "%";
		gameGUI.transform.Find("Money").transform.Find("Text").GetComponent<Text>().text = money.ToString("F1");
		gameGUI.transform.Find("Scrap").transform.Find("Text").GetComponent<Text>().text = scrap.ToString("F1");
		
		// update upgrade levels
		for (int i = 0; i < 4; i++){
			string whichUpgrade = "Upgrade" + (i+1);
			Debug.Log(whichUpgrade);
			int upgradeLevel = 0;
			if (availableUpgrades[i] == "Hull Fortification: +10 Max Hull"){
				upgradeLevel = hpUpgrade;
			}
			else if (availableUpgrades[i] == "Shield Strength: +5 Max Shield"){
				upgradeLevel = shieldUpgrade;
			}
			else if (availableUpgrades[i] == "Enhance Thrusters: +1 Turn Speed"){
				upgradeLevel = turnSpeedUpgrade;
			}
			else if (availableUpgrades[i] == "Enhance Rockets: +10 Acceleration"){
				upgradeLevel = accelerationUpgrade;
			}
			else if (availableUpgrades[i] == "Widen Fuel Tank: +100 Max Fuel"){
				upgradeLevel = fuelUpgrade;
			}
			else if (availableUpgrades[i] == "Fuel Efficiency: -10% Fuel Consumption"){
				upgradeLevel = fuelConsumptionSpeedUpgrade;
			}
			else if (availableUpgrades[i] == "Enhance Firepower: +5 Damage"){
				upgradeLevel = attackDamageUpgrade;
			}
			else if (availableUpgrades[i] == "Nanobot Upgrade: +0.3 Hull Heal Speed"){
				upgradeLevel = hullHealSpeedUpgrade;
			}
			else if (availableUpgrades[i] == "Enhance Battery: +0.5 Shield Heal Speed"){
				upgradeLevel = shieldHealSpeedUpgrade;
			}
			else if (availableUpgrades[i] == "Improved Reload: +10% Attack Speed"){
				upgradeLevel = timeBetweenAttacksUpgrade;
			}
			else if (availableUpgrades[i] == "Increased Cargo Space: +50 Cargo Space"){
				upgradeLevel = cargoSpaceUpgrade;
			}
			
			gameGUI.transform.Find(whichUpgrade).GetComponent<Text>().text = availableUpgrades[i] + "\nPrice for Level " +  (upgradeLevel+1) + ": " + (100*Mathf.Pow(1.5f, upgradeLevel));
		}
	}
	
}
