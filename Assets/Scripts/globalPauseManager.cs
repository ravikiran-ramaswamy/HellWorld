using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class globalPauseManager : MonoBehaviour {
	public GameObject escapeMenu; 
	public GameObject gameOverFade; // A fake menu for the sake of letting button colors animate. 
	public GameObject gameOverMenu;
	public GameObject gameWinMenu;
	public GameObject player;
	public GameObject monster;


	public Text youAreDead;
	public Text retry;
	public Text exit;

	private static globalPauseManager current;
	public float deathAnim = -1f; 
	private float deathAnimLength = 5f; 
	private bool pause = false;
	private bool allowPlayerInput = true;

	// Use this for initialization
	void Start () {
		current = this;

		Unpause ();
		allowPlayerInput = true;
		Cursor.visible = false;

		escapeMenu.SetActive (false);
		gameOverMenu.SetActive (false);
		gameOverFade.SetActive (false);
		gameWinMenu.SetActive (false);

//		youAreDead = gameOverMenu.transform.Find("You are Dead"); 
//		retry = gameOverMenu.Find ("Retry Button");
//		exit = gameOverMenu.Find ("Quit Button");
	}
	
	// Update is called once per frame
	void Update () {

        // Escape Menu Management
        if (!pause && allowPlayerInput && Input.GetButtonDown("Cancel"))
        {
            escapeMenu.SetActive(true);
            Pause();
        }
        else if (pause && allowPlayerInput && (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Back")))
        {
            escapeMenu.SetActive(false);
            Unpause();
        }

		// Death text animations
		if (deathAnim >= 0 && deathAnim <= deathAnimLength) {
			// Yes, it has to be done in three lines of code. Color Blocks seem rather antiquated and featureless. 
			youAreDead.color = Color.Lerp(Color.clear, Color.white, deathAnim * 2f / deathAnimLength); 
			retry.color = Color.Lerp(Color.clear, Color.white, (deathAnim -1) / (deathAnimLength -1));
			exit.color =  Color.Lerp(Color.clear, Color.white, (deathAnim -2) / (deathAnimLength -2)); 

			deathAnim += Time.deltaTime;

			if (deathAnim > deathAnimLength) { // Switch to the real menu once fading is over. 
				gameOverMenu.SetActive (true);
				gameOverFade.SetActive (false);
				deathAnim = -1f; 
			}
		}
		ColorBlock test = new ColorBlock ();
		test.normalColor = Color.white;
	}

	public static bool isPaused() {
		
		return current.pause;
	}

	public void Pause() {
		pause = true;
		UpdateActive ();
	}

	public void Unpause() {
		pause = false; 
		UpdateActive ();
	}

	public void triggerDefeat() {
		current.Pause ();

		youAreDead.color = Color.clear;
		retry.color = Color.clear;
		exit.color = Color.clear;


		gameOverFade.SetActive (true);
		allowPlayerInput = false;

		// Starts the text fading in. 
		deathAnim = 0f; 
	}

	public void triggerWin() {
		current.Pause ();
		gameWinMenu.SetActive (true);
	}

	private void UpdateActive() {
		Cursor.visible = pause;
		player.SetActive (!pause);
		monster.SetActive (!pause);
	}
}
