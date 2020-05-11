using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
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
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
            break;
            case "Finish":
            state = State.Transcending;
            Invoke("LoadNextScene", 1f); //parameterize time
            break;
            default:
            print("Dead");
            SceneManager.LoadScene(0);
            break;
        }
    }

    private void LoadNextScene() 
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //Can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying) //Prevent audio layering
            {
                audioSource.Play();
            }  
        }
        else 
            {
                audioSource.Stop();
            }
    }

    private void Rotate()
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