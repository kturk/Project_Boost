using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;// = new Vector3(0f, 0f, 0f);
    [SerializeField] float period;// = 0f;
    //[Range(0, 1)] [SerializeField] 
    float movementMultiplier;
    Vector3 startingPosition;
    private const float tau = Mathf.PI * 2f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
            return;
        float cycles = Time.time / period;
        movementMultiplier = Mathf.Sin(cycles * tau) / 2f + 0.5f; // Between 0 and 1
        Vector3 offset = movementMultiplier * movementVector;
        transform.position = startingPosition + offset;
    }
}
