using UnityEngine;
using System.Collections;

public class PortalInteraction : MonoBehaviour {
	public GameObject globalObject;
	public AudioSource source;
	public AudioClip spookClip;
	public AudioClip origClip;

	private bool playSpookSound = false;
	private globalPauseManager globalPauseManager;
	private globalMessager globalMessager;

	public int piecesNeeded = 5;
	private int piecesHeld = 0;

	private string messageEnd = " Pieces Remaining";


	// Use this for initialization
	void Start () {
		globalPauseManager = globalObject.GetComponent<globalPauseManager> ();
		globalMessager = globalObject.GetComponent<globalMessager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playSpookSound) {
			source.Stop ();
			source.clip = spookClip;
			source.loop = false;
			source.Play ();
			playSpookSound = false;
		}
		if (!source.isPlaying) {
			source.Stop ();
			source.clip = origClip;
			source.loop = true;
			source.Play ();
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "VictoryPiece" && Vector3.Distance(transform.position, collider.gameObject.transform.position) < 3.0f) {
			Destroy (collider.gameObject);
			piecesHeld++;
			globalMessager.showMessage ((piecesNeeded - piecesHeld) + messageEnd);
			playSpookSound = true;
		}
		if (collider.gameObject.tag == "Portal") {
			if (piecesHeld >= piecesNeeded) {
				globalPauseManager.triggerWin ();
			} else {
				globalMessager.showMessage ((piecesNeeded - piecesHeld) + messageEnd);
			}
		}
	}
}
