using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour {
	// keep track of the upgrades bought, and then
	// save them when user leaves
	public float scrapToSell;
	public float buyPrice;
	
	public void CalculateValue(){
		scrapToSell = float.Parse(GameObject.Find("Canvas").transform.Find("Sell").transform.Find("InputField").GetComponent<InputField>().text);
		buyPrice = GameObject.Find("StatRetriever").GetComponent<PlanetManager>().currentBuyPrice;
		GameObject.Find("Canvas").transform.Find("Sell").transform.Find("SellButton").transform.Find("Text").GetComponent<Text>().text = "Sell for: " + (scrapToSell*buyPrice).ToString("F1");
		
		// update the sell button's script as well
		GameObject.Find("Canvas").transform.Find("Sell").transform.Find("SellButton").GetComponent<UpgradeManager>().scrapToSell = scrapToSell;
		GameObject.Find("Canvas").transform.Find("Sell").transform.Find("SellButton").GetComponent<UpgradeManager>().buyPrice = buyPrice;
	}
	
	// player gains money and fame for selling scrap
	public void SellScrap(){
		PlanetManager planetManager = GameObject.Find("StatRetriever").GetComponent<PlanetManager>();
		if (scrapToSell > 0 && scrapToSell <= planetManager.scrap){
			planetManager.scrap -= scrapToSell;
			planetManager.money += (int)(scrapToSell*buyPrice);
			planetManager.playerFame += scrapToSell/500f;
		}
		PlayerPrefs.SetFloat("Scrap", planetManager.scrap);
		PlayerPrefs.SetFloat("Money", planetManager.money);
	}
	
	public void BackToGame(){
		GameObject.Find("StatRetriever").GetComponent<PlanetManager>().SaveUpgrades();
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
	
	public void UpgradeItem(){
		string buttonText = transform.parent.GetComponent<Text>().text;
		PlanetManager planetManager = GameObject.Find("StatRetriever").GetComponent<PlanetManager>();
		
		// need to upgrade the GUI as well as save 
		if (buttonText.Contains("Hull Fortification: +10 Max Hull")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.hpUpgrade)){
				planetManager.hpUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.hpUpgrade);
			}
			
		}
		else if (buttonText.Contains("Shield Strength: +5 Max Shield")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.shieldUpgrade)){
				planetManager.shieldUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.shieldUpgrade);
			}
		}
		else if (buttonText.Contains("Enhance Thrusters: +1 Turn Speed")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.turnSpeedUpgrade)){
				planetManager.turnSpeedUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.turnSpeedUpgrade);
			}
		}
		else if (buttonText.Contains("Enhance Rockets: +10 Acceleration")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.accelerationUpgrade)){
				planetManager.accelerationUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.accelerationUpgrade);
			}
		}
		else if (buttonText.Contains("Widen Fuel Tank: +100 Max Fuel")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.fuelUpgrade)){
				planetManager.fuelUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.fuelUpgrade);
			}
		}
		else if (buttonText.Contains("Fuel Efficiency: -10% Fuel Consumption")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.fuelConsumptionSpeedUpgrade)){
				planetManager.fuelConsumptionSpeedUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.fuelConsumptionSpeedUpgrade);
			}
		}
		else if (buttonText.Contains("Enhance Firepower: +5 Damage")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.attackDamageUpgrade)){
				planetManager.attackDamageUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.attackDamageUpgrade);
			}
		}
		else if (buttonText.Contains("Nanobot Upgrade: +0.3 Hull Heal Speed")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.hullHealSpeedUpgrade)){
				planetManager.hullHealSpeedUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.hullHealSpeedUpgrade);
			}
		}
		else if (buttonText.Contains("Enhance Battery: +0.5 Shield Heal Speed")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.shieldHealSpeedUpgrade)){
				planetManager.shieldHealSpeedUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.shieldHealSpeedUpgrade);
			}
		}
		else if (buttonText.Contains("Improved Reload: +10% Attack Speed")){
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.timeBetweenAttacksUpgrade)){
				planetManager.timeBetweenAttacksUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.timeBetweenAttacksUpgrade);
			}
		}
		else if (buttonText.Contains("Increased Cargo Space: +50 Cargo Space")){	
			planetManager.cargoSpaceUpgrade++;
			if (planetManager.money >= 100*Mathf.Pow(1.5f, planetManager.cargoSpaceUpgrade)){
				planetManager.cargoSpaceUpgrade++;
				planetManager.money -= 100*Mathf.Pow(1.5f, planetManager.cargoSpaceUpgrade);
			}
		}
		planetManager.SaveUpgrades();
	}
}
