using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 75f; //public brings to inspector, but makes it changeable from other scripts
    [SerializeField] float thrustPower = 10f;
    [SerializeField] float loadDelay = 1.5f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip newLevelSound;
    [SerializeField] AudioClip playerDeathSound;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToThrustInput()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            engineParticles.Stop();
        }

    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustPower * Time.deltaTime); //relative to its orientation

        if (!audioSource.isPlaying)//if not playing is the exclamation point
        {
            audioSource.PlayOneShot(mainEngine);
        }
        engineParticles.Play();
    }

    private void RespondToRotateInput()
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
        if (state != State.Alive)
        {
            return; // if we are not alive, just don't bother with the below}
        }

            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    print("OK"); //nothing happens intentionally
                    break;
                case "Finish":
                    StartSuccessSequence();
                    break;
                //case "Fuel":
                //print("Fuel");
                //break;
                default:
                StartDeathSequence();
                    break;
            }
    }

    void StartSuccessSequence()
    {
        print("You did it");
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(newLevelSound);
        successParticles.Play();
        Invoke("LoadNextScene", loadDelay); //do the "load next scene function after loadDelay amount of time
    }

    void StartDeathSequence()
    {
        print("DEAD!!");
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(playerDeathSound);
        deathParticles.Play();
        Invoke("LoadCurrentScene", loadDelay);
    }

    void LoadCurrentScene()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }


}