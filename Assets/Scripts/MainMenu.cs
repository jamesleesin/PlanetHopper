using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public void NewGame(){
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
	
	public void Instructions(){
		SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
	}
	
	public void Back(){
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
