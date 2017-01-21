using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class globalMessager : MonoBehaviour {

	public GameObject msg;
	public GameObject msgShadow;

	public Color msgColor = new Color (1f, 1f, 1f, 1f);
	public Color msgShadowColor = new Color (0f, 0f, 0f, 1f);

	private Text msgText;
	private Text msgTextShadow;

	private Color msgColorClear; 
	private Color msgShadowColorClear; 

	private bool isAnimating = false;
	private float anim = 0f;
	private float fadeTime = 0.5f;
	private float visibleTime;
	private float defaultVisibleTime = 4f;

	// Use this for initialization
	void Start () {
		msgText = msg.GetComponent<Text> ();
		msgTextShadow = msgShadow.GetComponent<Text> ();

		msgColorClear = msgColor;
		msgColorClear.a = 0f;
		msgShadowColorClear = msgShadowColor;
		msgShadowColorClear.a = 0f;

		msgText.color = msgColorClear;
		msgTextShadow.color = msgShadowColorClear;

		msg.SetActive (true);
		msgShadow.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimating) {
			anim += Time.deltaTime;
			if (anim <= fadeTime || anim >= fadeTime + visibleTime) {
				float fadePercent; 
				if (anim <= fadeTime) {
					fadePercent = anim / fadeTime;
				} else {
					fadePercent = 1 - ((anim - visibleTime - fadeTime) / fadeTime);
				}
				//Mathf.Lerp (0f, 1f, fadePercent);
				msgText.color = Color.Lerp(msgColorClear, msgColor, fadePercent);
				msgTextShadow.color = Color.Lerp(msgShadowColorClear, msgShadowColor, fadePercent);
			}
			if (anim >= fadeTime * 2 + visibleTime) {
				isAnimating = false;
				anim = 0f; 
				msgText.color = msgColorClear;
				msgTextShadow.color = msgShadowColorClear;
			}
		}
	}

	public void showMessage(string text) {
		showMessage (text, defaultVisibleTime);
	}

	// Duration is in seconds. 
	// Printing a new message while one is in progress overwrites the current message. 
	public void showMessage(string text, float duration) {
		visibleTime = duration;
		msgText.text = text;
		msgTextShadow.text = text; 
		isAnimating = true;

		// If message is in progress, reset duration and alpha value. 
		if (anim > fadeTime) {
			anim = fadeTime; 
			msgText.color = msgColor;
			msgTextShadow.color = msgShadowColor; 
		}
	}
}
