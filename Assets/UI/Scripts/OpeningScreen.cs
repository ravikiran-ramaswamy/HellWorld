//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;

public class OpeningScreen : MonoBehaviour {

    public GameObject Panel;
    private IEnumerator coroutine;
    // Use this for initialization
    void Start () {
        coroutine = WaitAndPrint(5.0f);
        StartCoroutine(coroutine);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Panel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
