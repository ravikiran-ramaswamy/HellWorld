using UnityEngine;
using System.Collections;

public class StopAudio : MonoBehaviour {

	public AudioSource horrorSource;
	public AudioSource monsterSource;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name.Equals("creature1")) {
			horrorSource.volume = 0f;
			monsterSource.volume = 0f;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.name.Equals("creature1")) {
			horrorSource.volume = 0.7f;
			monsterSource.volume = 1.0f;
		}
	}
}
