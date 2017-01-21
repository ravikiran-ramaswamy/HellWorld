using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerDeath : MonoBehaviour {
	public GameObject monster;
	public GameObject globalObject;
	public GameObject flashlight;
	public GlobalFog fogScript;
	public WorldShift shiftScript;
	public float deathDistance = 2f;
	public float fogStartDistance = 5f;
	public Color fogDeathColor = new Color(1f, 0f, 0f, 0.5f);

	private globalPauseManager globalPauseManager;
	private bool dead = false;
	private Vector3 distance;
	private float fogMaxDistance; 
	private Color fogNormalColor; 

	// Use this for initialization
	void Start () {
		globalPauseManager = globalObject.GetComponent<globalPauseManager> ();
		fogMaxDistance = fogScript.startDistance;
		fogNormalColor = RenderSettings.fogColor;
		fogStartDistance += fogMaxDistance;
	}
	
	// Update is called once per frame
	void Update () {
		distance = gameObject.transform.position;
		distance -= monster.transform.position;

		if (!shiftScript.parallel && !flashlight.activeSelf && distance.magnitude > fogStartDistance) {
			flashlight.SetActive (true);
			fogScript.startDistance = fogMaxDistance; 
			RenderSettings.fogColor = fogNormalColor;
		}
		if (!shiftScript.parallel && distance.magnitude <= fogStartDistance) {
			// LERP fog level between start and closing distance. 
			float percent = distance.magnitude / fogStartDistance;

			fogScript.startDistance = Mathf.Lerp(0f, fogMaxDistance, percent);
			RenderSettings.fogColor = Color.Lerp (fogDeathColor, fogNormalColor, percent);
//			flashlight.SetActive (false);
		}
		if (distance.magnitude <= deathDistance && !dead) {
			globalPauseManager.triggerDefeat ();
			dead = true;
		}
	}
}
