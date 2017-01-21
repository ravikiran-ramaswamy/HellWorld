//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public CharacterController controller;
	public float speed = 0.2f;
	public Vector3 direction; 

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		direction = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical") );
		direction.Normalize();
		direction *= speed;
		controller.Move (direction);
	}
}
