using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource engineAudio;
    [SerializeField] float rcsThrust;
    [SerializeField] float mainThrust;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody   = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();
        rcsThrust   = 100f;
        mainThrust  = 25f;
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
                print("OK"); // TODO remove
                break;
            default:
                print("dead"); // remove and add more logic
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!engineAudio.isPlaying)
                engineAudio.Play();
        }
        else
            engineAudio.Stop();
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // Taking control of rocket.
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * rotationThisFrame);

        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        
        rigidBody.freezeRotation = false; // Resume normal physics control.
    }
}
