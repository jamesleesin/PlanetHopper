using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	public float worldPositionX;
	public float worldPositionY;
	public string planetName;
	public int planetID;
	public int affection = 0;
	public Rigidbody2D[] rigidbodies;
	public float planetSize;
	public Rigidbody2D planrb;
	private const float gravitationalConst = 0.00000667f;
	public Sprite[] planetImages;
	
	public void Initialize (string name, float posX, float posY, float size) {
		planetName = name;
		worldPositionX = posX;
		worldPositionY = posY;
		transform.position = new Vector3(worldPositionX, worldPositionY, 0);
		affection = 0;
		planetSize = size;
		transform.localScale = new Vector2(planetSize, planetSize);
		planrb = GetComponent<Rigidbody2D>();
		planrb.mass = 100f * (4f/3f) * Mathf.PI * Mathf.Pow(size*2, 3f);
		
		if (planetName == "Earth"){
			GetComponent<SpriteRenderer>().sprite = planetImages[0];
		}
		else if (planetName == "Mars"){
			GetComponent<SpriteRenderer>().sprite = planetImages[1];
		}
		else if (planetName == "Galio"){
			GetComponent<SpriteRenderer>().sprite = planetImages[2];
		}
		else{
			GetComponent<SpriteRenderer>().sprite = planetImages[3];
		}
	}
	
	// planet gravity
	// other things dont need gravity, we treat it as negligable
	void Update(){
		rigidbodies = (Rigidbody2D[]) GameObject.FindObjectsOfType (typeof(Rigidbody2D));
		// gravity, suck all rigidbodies towards itself
		// Gmm/r^2
		// do some gravity "helping", if distance is too far do nothing,
		// if distance short then add a multiplier
		foreach(Rigidbody2D rb in rigidbodies){
			// dont act on itself
			if (rb != planrb){
				float force;
				// if distance is too short then dont do anything
				if (Vector3.Distance(transform.position, rb.position) < 2f || Vector3.Distance(transform.position, rb.position) > 200f){
					force = 0f;
				}
				else if (Vector3.Distance(transform.position, rb.position) < 10f){
					force = (10f * gravitationalConst * rb.mass * planrb.mass)/(Mathf.Pow(Vector3.Distance(transform.position, rb.position), 2f));		
				}
				else{
					force = (gravitationalConst * rb.mass * planrb.mass)/(Mathf.Pow(Vector3.Distance(transform.position, rb.position), 2f));
				}
				Vector3 newVector = transform.position - new Vector3(rb.position.x, rb.position.y, 0f);
				rb.AddForce(newVector * force);
			}
		}
	}
}
