//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;

public class DollyCam : MonoBehaviour {

	private bool dollyOn = false;

	public Vector3 startPos = new Vector3(0.3f, 0f, 0.2f);
	public float startFov = 30f;

	public Vector3 endPos = new Vector3(0f, 0f, 0f);
	public float endFov  = 0f;
	public float maxTime = 3f;

	private float time = 0f;

	public Vector3 dollyCamOffset; 
	public float dollyFovOffset;


	// Use this for initialization
	void Start () {
		
	}

	// Call in order to enable Dolly calculations. 
	public void startDolly() {
		dollyOn = true;
		time = 0f;
		resetDolly ();
	}

	public void endDolly() {
		dollyOn = false;
		// Depending on what we want to happen to the player camera, it might be worth resetting the camera transform here. 
		resetDolly();
	}

	public void resetDolly() {
		dollyCamOffset = Vector3.zero;
		dollyFovOffset = 0f;
	}

	// Call every frame in order to compute the dollyCamOffset and dollyFovOffset variables. 
	public void renderDolly() {
		if (dollyOn) {
			time += Time.deltaTime;
			float timePercent = time / maxTime;
			timePercent = timePercent * timePercent * timePercent;

			dollyCamOffset = Vector3.Lerp (startPos, endPos, timePercent);
			dollyFovOffset = Mathf.Lerp (startFov, endFov, timePercent);
		}
	}
}
