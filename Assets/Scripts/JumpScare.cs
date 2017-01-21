using UnityEngine;
using System.Collections;

public class JumpScare : MonoBehaviour {

	public Camera mainCam;
	public GameObject monster;
	public AudioSource horrorSource;
	public AudioClip jumpScare;

	private bool playSound = false;

	void Update () {
		Vector3 screenPoint = mainCam.WorldToViewportPoint(monster.transform.position);
		bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

		if (onScreen && playSound) {
			horrorSource.volume = 1.0f;
			horrorSource.PlayOneShot (jumpScare);
			playSound = false;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name.Equals("creature1")) {
			playSound = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.name.Equals("creature1")) {
			playSound = false;
		}
	}

}
