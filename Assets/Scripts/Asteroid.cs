using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public float hp;
	public float size;
	public float movementSpeed;
	public Vector3 movementDirection;
	private Vector3 camPosNoZ;
	public Asteroid asteroidPrefab;

	public void Initialize(float s, float startX, float startY){
		size = s;
		transform.position = new Vector3(startX, startY, 0);
		transform.localScale = new Vector2(size, size);
		movementSpeed = Random.Range(0.1f, 3f);
		movementDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
		
		// hp scales with size
		hp = 20*Mathf.Pow(2, size);
		// change mass according to size
		Rigidbody2D astrb = GetComponent<Rigidbody2D>();
		astrb.mass = (4f/3f) * Mathf.PI * Mathf.Pow(size, 3f);
	}
	
	void Death(bool killedByPlayer){
		if (size < 1f){
			// nothing
		}
		else{
			GameObject gm = GameObject.Find("GameManager");
			// split into pieces and give scrap
			int pieces;
			if (size < 3f){
				// 1 or 2 pieces
				pieces = Random.Range(1, 3);
			}
			else {
				// 2 or 3 pieces
				pieces = Random.Range(2, 4);
			}
			for (int i = 0; i < pieces; i++){
				float newSize = size/2f;
				gm.GetComponent<GameManager>().SpawnSmallAsteroid(newSize, transform.position.x, transform.position.y);
			}
		}
		// give scrap
		if (killedByPlayer){
			GameObject.Find("Player(Clone)").GetComponent<Player>().GainScrap(Mathf.Pow(2f, size));
		}
		Destroy(this.gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D obj){
		if (obj.tag == "Attack"){
			Attack atk = obj.GetComponent<Attack>();
			hp -= atk.damage;
			// if its the players attack then give scrap on death
			if (hp <= 0){
				if (GameObject.Find("Player(Clone)").GetComponent<Player>().attackList.Contains(atk)){
					Death(true);
				}
				else{
					Death(false);
				}
			}
			Destroy(obj.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
		
		camPosNoZ = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
		// destroy itself if its very far outside of the camera
		if(Vector3.Distance(camPosNoZ, transform.position) > 100f){
			Destroy(this.gameObject);
		}
	}
}
