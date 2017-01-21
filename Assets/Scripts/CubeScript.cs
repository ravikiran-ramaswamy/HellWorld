using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {

    private Renderer renderer;
	
    // Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        renderer.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
    }

    void OnMouseExit()
    {
        renderer.material.shader = Shader.Find("Diffuse");
    }
}
