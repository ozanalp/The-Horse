using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float power = .2f;
    public float duration = .2f;
    public float slowDownAmount = 1f;
    private bool should_Shake = false;
    private float initialDuration;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
        initialDuration = duration;
    }

    private void Update()
    {
        Shake();
    }

    private void Shake()
    {
        if (should_Shake)
        {
            if (duration > 0)
            {
                transform.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                // reset the duration and set shaking off
                should_Shake = false;
                duration = initialDuration;
                transform.localPosition = startPosition; // we stored the initial position in Start()
            }
        }
    }
    // accessor function to access private values
    public bool ShouldShake
    {
        get { return should_Shake; }
        set { should_Shake = value; }
    }
}
