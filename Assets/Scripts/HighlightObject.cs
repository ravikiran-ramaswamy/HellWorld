using UnityEngine;
using System.Collections;

public class HighlightObject : MonoBehaviour {

    private GameObject lookedAt;

	// Use this for initialization
	void Start () {
        lookedAt = new GameObject();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ViewportPointToRay(transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.transform.gameObject;
            Debug.Log(obj.ToString());
            if (obj != lookedAt)
            {
                Debug.Log("Obj: " + obj.ToString());
                Debug.Log("lookedAt: " + lookedAt.ToString());
                if (lookedAt.GetComponent<Renderer>())
                    lookedAt.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
                if (obj.tag == "Pickable")
                    obj.GetComponent<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
                lookedAt = obj;
            }
        }
        else
            Debug.Log("I'm looking at nothing!");

    }
}
