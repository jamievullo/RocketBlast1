using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); //generics deep C# 
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { // need to stop sound on death todo
        if (state == State.Alive) 
        {
        RespondToThrustInput();
        RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //parameterize time
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                Invoke("LoadFirstLevel", 1f); //parameterize time
                break;
        }
        
    }

    private void LoadNextLevel() 
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel() 
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) //Can thrust while rotating
        {
            ApplyThrust();
        }
        else 
            {
                audioSource.Stop();
            }
    }

    private void ApplyThrust() 
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying) //Prevent audio layering
        {
            audioSource.PlayOneShot(mainEngine);
        }  
    }

    private void RespondToRotateInput()
    {     

        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }
}