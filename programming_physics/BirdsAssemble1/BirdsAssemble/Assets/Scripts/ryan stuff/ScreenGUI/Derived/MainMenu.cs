using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public Text Score;

	public void LoadScene(int index) {
		// load the next scene into scene manager
		SceneManager.LoadScene(index);
	}
	public void LoadScene(string name) {
		// load the next scene into scene manager
		SceneManager.LoadScene(name);
	}

	// if play button hit, play the game scene
	public void LoadNextScene() {
		// load the next scene into scene manager
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void PlayAgain() {
		// load the next scene into scene manager
		SceneManager.LoadScene("BossBattle");
	}

	public void QuitGame() {
		// load the next scene into scene manager
		Application.Quit();
	}

	public void Start() {
		if (Score) {
			Score.text = "SCORE: " + PlayerPrefs.GetFloat("Score", 100);
		}
	}
}
