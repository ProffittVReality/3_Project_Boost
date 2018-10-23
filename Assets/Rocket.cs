using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    { 
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up); //relative to its orientation
        }

        if (Input.GetKey(KeyCode.A)) //the double if statements here lets it thrust and turn at the same time.
        {
           transform.Rotate(Vector3.forward);
        }

        else if (Input.GetKey(KeyCode.D)) //Only A and D are mutually exclusive
        {
            transform.Rotate(-Vector3.forward);
        }

    }
}
