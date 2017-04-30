using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	private Vector3 camPosNoZ;
	private Vector3 movement;
	public float damage;
	
	// Use this for initialization
	void Start () {
		Vector3 camPos = Camera.main.transform.position;
	}
	
	public void Initialize(float posX, float posY, Vector3 vel, float dmg){
		transform.position = new Vector3(posX, posY, -0.5f);
		movement = vel;
		damage = dmg;
		// color based on damage
		if (dmg < 50f){
			GetComponent<Renderer>().material.color = new Color(0f, 1f, 1f-(dmg*0.02f));
		}
		else if (dmg < 100) {
			GetComponent<Renderer>().material.color = new Color((dmg-50f)*0.02f, 1f, 0f);
		}
		else if (dmg < 150){
			GetComponent<Renderer>().material.color = new Color(1f, 1f-(dmg*0.02f), 0f);
		}
		else{
			GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(movement * Time.deltaTime);
		
		camPosNoZ = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
		// destroy itself if its very far outside of the camera
		if(Vector3.Distance(camPosNoZ, transform.position) > 50f){
			Destroy(this.gameObject);
		}
	}
}
