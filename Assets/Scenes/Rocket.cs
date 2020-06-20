using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem engineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;

    [SerializeField] float rcsThrust;
    [SerializeField] float mainThrust;
    [SerializeField] float levelLoadDelay;

    enum State { Alive, Dying, Transcending }
    [SerializeField] State state;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody   = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rcsThrust   = 250f;
        mainThrust  = 1000f;
        levelLoadDelay = 1.5f;
        state = State.Alive;
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

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) // Ignore collision when the player is already dead
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticle.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticle.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // TODO should keep current level and load currentLevel+1
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
            ApplyThrust();
        else
        {
            audioSource.Stop();
            engineParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(engineSound);
        engineParticle.Play();
    }

    private void RespondToRotateInput()
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
