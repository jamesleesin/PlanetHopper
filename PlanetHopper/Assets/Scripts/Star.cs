using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	private Vector3 camPosNoZ;

	
	// Use this for initialization
	void Start () {
	}
	
	public void Initialize(float posX, float posY, float size, Color color){
		transform.position = new Vector3(posX, posY, 0f);
		transform.localScale = new Vector2(size, size);

		GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		camPosNoZ = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
		// destroy the star if it is too far from the camera
		if(Vector3.Distance(camPosNoZ, transform.position) > 50f){
			Destroy(this.gameObject);
		}
	}
}
