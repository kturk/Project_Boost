using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    // TODO Fix: audio keeps running after death
    Rigidbody rigidBody;
    AudioSource engineAudio;
    [SerializeField] float rcsThrust;
    [SerializeField] float mainThrust;

    enum State { Alive, Dying, Transcending }
    [SerializeField] State state;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody   = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();
        rcsThrust   = 150f;
        mainThrust  = 30f;
        state = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) 
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1.5f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1.5f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // TODO should keep current level and load currentLevel+1
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
