//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour {

    //public GameObject loadingImage;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void LoadScene(int level)
    {
        //loadingImage.SetActive(true);
        //Application.LoadLevel(level);
        SceneManager.LoadScene(level);
    }
}
