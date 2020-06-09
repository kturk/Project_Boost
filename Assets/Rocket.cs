using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource engineAudio;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!engineAudio.isPlaying)
                engineAudio.Play();
        }
        else 
        {
            engineAudio.Stop();
        }

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward);

        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.forward);
    }
}
