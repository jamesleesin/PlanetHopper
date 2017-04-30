using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private float gameKey = 10f;
	public float cleanupTimer = 0f;
	
	public Player playerPrefab;
	public Asteroid asteroidPrefab;
	public Planet planetPrefab;
	public Star starPrefab;
	public List<Asteroid> asteroidList = new List<Asteroid>();
	public List<Planet> planetList = new List<Planet>();
	public List<Star> starList = new List<Star>();
	public Pirate piratePrefab;
	public List<Pirate> pirateList = new List<Pirate>();
	
	private float spawnDistanceFromPlayer = 8f;
	private float cameraRange = 20;
	private List<Color> starColors = new List<Color>();
	
	// list of planet names
	private string[] planetNames ={"Earth", "Mars", "Alpha", "Beta", "Epsilon", "Delta", "Galio", "Orpheus", "Gaea", "Sel", "Sirius"};
	
	// asteroid spawn
	private float asteroidSpawnDistFromPlayer = 30f;
	private float asteroidTimeBetweenSpawns;
	private float asteroidSpawnTimer;
	
	// star spawn
	private float starTimeBetweenSpawns;
	private float starSpawnTimer;
	
	// player
	GameObject playerGO;
	Vector3 playerVel;
	Vector3 camPos;
	Player player;
	
	// Use this for initialization
	void Start () {
		// spawn the player and the asteroids
		player = Instantiate(playerPrefab) as Player;
		for (int i = 0; i < 50; i++){
			SpawnAsteroid();
		}
		
		for (int i = 0; i < 10; i++){
			// planet locations based on game key
			Planet plan = Instantiate(planetPrefab) as Planet;
			// perlin noise between 0-1
			float planetX = 2000f*Mathf.PerlinNoise(planetNames[i].Length/gameKey, (i+1)/gameKey) - 1000f;
			float planetY = 2000f*Mathf.PerlinNoise((i+1)/gameKey, gameKey*i) - 1000f;
			float size = Mathf.PerlinNoise((i+1)/(gameKey+planetNames[i].Length), (i+planetNames[i].Length)/planetNames[i].Length)*1.5f + 1.5f;
			plan.Initialize(planetNames[i], planetX, planetY, size);
			planetList.Add(plan);
		}
		
		asteroidTimeBetweenSpawns = 1f;
		asteroidSpawnTimer = 0f;
		starTimeBetweenSpawns = 0.1f;
		starSpawnTimer = 0f;
		
		starColors.Add(new Color(1f, 1f, 1f)); //white
		starColors.Add(new Color(1f, 1f, 0f)); //yellow
		starColors.Add(new Color(1f, 0.35f, 0.35f)); //red
		
		SpawnInitialStars();
		playerGO = GameObject.Find("Player(Clone)");
		
		// pirates!
		/*
		Pirate pirate = Instantiate(piratePrefab) as Pirate;
		pirate.transform.position = new Vector3(-83f, 54f, 0f);
		pirateList.Add(pirate);*/
	}
	
	void SpawnInitialStars(){
		// get player spawn
		float initX = PlayerPrefs.GetFloat("PlayerX");
		float initY = PlayerPrefs.GetFloat("PlayerY");
		// spawn stars
		for (int i = 0; i < 200; i++){
			Star star = Instantiate(starPrefab) as Star;
			star.Initialize(Random.Range(initX-cameraRange, initX+cameraRange), Random.Range(initY-cameraRange, initY+cameraRange), Random.Range(0.1f, 1f), starColors[Random.Range(0, starColors.Count)]);
			
			starList.Add(star);
		}
	}
	
	public void SpawnSmallAsteroid(float s, float startX, float startY){
		Asteroid ast = Instantiate(asteroidPrefab) as Asteroid;
		ast.Initialize(s, startX, startY);
		asteroidList.Add(ast);
	}
	
	void SpawnAsteroid(){
		if (asteroidList.Count < 200f){
			Asteroid ast = Instantiate(asteroidPrefab) as Asteroid;
			float probability = Random.Range(0f, 100f);
			// chance for larger asteroid is smaller
			float size;
			// 10% chance for small asteroid
			if (probability < 10f){
				size = 0.5f;
			}
			// 40% chance for normal asteroid
			else if (probability < 50f){
				size = 1f;
			}
			// 40% chance for slightly larger asteroid
			else if (probability < 90f){
				size = 2f;
			}
			// 8% chance for large asteroid
			else if (probability < 98f){
				size = 3.5f;
			}
			// 2% chance for massive asteroid
			else{
				size = 5f;
			}
			// a little bit of variance
			size += Random.Range(0f, 0.5f);
			
			// set x and y outside of camera range
			Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y,0f);
			float startX = Random.Range(-asteroidSpawnDistFromPlayer, asteroidSpawnDistFromPlayer);
			float startY = Random.Range(-asteroidSpawnDistFromPlayer,asteroidSpawnDistFromPlayer);
			while (startX >= -10f && startX <= 10f && startY >= -10f && startY <= 10f){
				startX = Random.Range(-asteroidSpawnDistFromPlayer, asteroidSpawnDistFromPlayer);
				startY = Random.Range(-asteroidSpawnDistFromPlayer, asteroidSpawnDistFromPlayer);
			}
			// move to where the player is
			startX += playerPos.x;
			startY += playerPos.y;
			ast.Initialize(size, startX, startY);
			
			asteroidList.Add(ast);
		}
	}
	
	void SpawnStar(){
		if (starList.Count < 300f){
			Star star = Instantiate(starPrefab) as Star;
			starList.Add(star);

			camPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
			float startX = Random.Range(-cameraRange, cameraRange);
			float startY = Random.Range(-cameraRange, cameraRange);
			while (startX >= -15f && startX <= 15f && startY >= -15f && startY <= 15f){
				startX = Random.Range(-cameraRange, cameraRange);
				startY = Random.Range(-cameraRange, cameraRange);
			}
			float size = Random.Range(0.1f, 1f);
			Vector3 starSpawnLoc = new Vector3(0f, 0f, 0f);
			if (playerVel.x == 0 && playerVel.y == 0){
				// nothing
			}
			// zero cases
			else if (playerVel.x == 0 && playerVel.y < 0){
				starSpawnLoc = new Vector3(camPos.x - startX,  camPos.y - spawnDistanceFromPlayer - startY, 0f);
			}
			else if (playerVel.x == 0 && playerVel.y > 0){
				starSpawnLoc = new Vector3(camPos.x - startX,  camPos.y + spawnDistanceFromPlayer + startY, 0f);
			}
			else if (playerVel.x < 0 && playerVel.y == 0){
				starSpawnLoc = new Vector3(camPos.x - spawnDistanceFromPlayer - startX,  camPos.y - startY, 0f);
			}
			else if (playerVel.x > 0 && playerVel.y == 0){
				starSpawnLoc = new Vector3(camPos.x + spawnDistanceFromPlayer + startX,  camPos.y - startY, 0f);
			}
			// both non zero
			else if (playerVel.x < 0 && playerVel.y < 0){
				starSpawnLoc = new Vector3(camPos.x - spawnDistanceFromPlayer - startX,  camPos.y - spawnDistanceFromPlayer - startY, 0f);
			}
			else if (playerVel.x > 0 && playerVel.y < 0){
				starSpawnLoc = new Vector3(camPos.x + spawnDistanceFromPlayer + startX,  camPos.y - spawnDistanceFromPlayer - startY, 0f);
			}
			else if (playerVel.x < 0 && playerVel.y > 0){
				starSpawnLoc = new Vector3(camPos.x - spawnDistanceFromPlayer - startX,  camPos.y + spawnDistanceFromPlayer + startY, 0f);
			}
			else if (playerVel.x > 0 && playerVel.y > 0){
				starSpawnLoc = new Vector3(camPos.x + spawnDistanceFromPlayer + startX,  camPos.y + spawnDistanceFromPlayer + startY, 0f);
			}
			star.transform.position = starSpawnLoc;
			star.transform.localScale = new Vector2(size, size);

			star.transform.GetComponent<Renderer>().material.color = starColors[Random.Range(0, starColors.Count)];
		}
	}
	
	// Update is called once per frame
	void Update () {
		// spawn asteroids periodically from outside vision range
		if (asteroidSpawnTimer <= 0f){
			SpawnAsteroid();
			asteroidSpawnTimer = asteroidTimeBetweenSpawns;
		}
		else{
			asteroidSpawnTimer -= Time.deltaTime;
		}
		// spawn stars outside the players FOV as the player moves
		// need to call this more times the faster the player is
		if (starSpawnTimer <= 0f){
			starSpawnTimer = starTimeBetweenSpawns;
			playerVel = playerGO.transform.GetComponent<Rigidbody2D>().velocity;
			float starMultiplier = playerVel.magnitude;
			for (float i = 0; i < starMultiplier; i++)
				SpawnStar();
		}
		else{
			starSpawnTimer -= Time.deltaTime;
		}

		// cleanup the asteroid list every 5 seconds
		if (cleanupTimer <= 0){
			cleanupTimer = 5f;
			for(int i = asteroidList.Count - 1; i > -1; i--)
			{
				if (asteroidList[i] == null)
				   asteroidList.RemoveAt(i);
			}
		}
		else{
			cleanupTimer -= Time.deltaTime;
		}
		
	}
}
