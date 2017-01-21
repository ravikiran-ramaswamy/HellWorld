using UnityEngine;
using System.Collections;

public class ObjectScript : MonoBehaviour {

    private Rigidbody rigidBody;
	// Use this for initialization
	void Start () {
        rigidBody = transform.gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rigidBody != null)
        {
            if (rigidBody.velocity.magnitude == 0)
            {
                rigidBody.isKinematic = true;
            }
        }
	}
}
