using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 75f; //public brings to inspector, but makes it changeable from other scripts
    [SerializeField] float thrustPower = 10f;

    Rigidbody rigidBody;
    AudioSource audioSource;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
	}

    private void Thrust()
    {

        //float thrustThisFrame = thrustPower * Time.deltaTime; //this time delta time makes it frame rate independent 

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustPower); //relative to its orientation
            if (!audioSource.isPlaying)//if not playing is the exclamation point
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // before take manual controll of rotation, freeze it so it doesn't spin out of controll

        float rotationThisFrame = rcsThrust * Time.deltaTime; //this time delta time makes it frame rate independent 

        if (Input.GetKey(KeyCode.A)) //the double if statements here lets it thrust and turn at the same time.
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D)) //Only A and D are mutually exclusive
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // physics engine takes over when you aren't pressing buttons

    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        { 
            case "Friendly":
            print("OK");
            break;
        //case "Fuel":
            //print("Fuel");
            //break;
        default:
            print("DEAD!!");
            break;
        }
    }

}
