using UnityEngine;

public class RicePouringManager : MonoBehaviour
{
    public ParticleSystem ricePouringParticles;   // Particle effect for rice grains
    public Transform bowlTransform;               // The bowl holding the rice
    public float pouringAngleThreshold = 45f;     // Angle at which rice starts pouring
    private bool isPouring = false;               // Tracks if rice is currently pouring

    void Update()
    {
        // Calculate the tilt angle of the bowl
        float tiltAngle = Vector3.Angle(bowlTransform.up, Vector3.up);

        // Check if the bowl is tilted enough to start pouring
        if (tiltAngle > pouringAngleThreshold && !isPouring)
        {
            StartPouring();
        }
        else if (tiltAngle <= pouringAngleThreshold && isPouring)
        {
            StopPouring();
        }
    }

    private void StartPouring()
    {
        if (ricePouringParticles != null && !ricePouringParticles.isPlaying)
        {
            ricePouringParticles.Play();
            isPouring = true;
            Debug.Log("Rice pouring started.");
        }
    }

    private void StopPouring()
    {
        if (ricePouringParticles != null && ricePouringParticles.isPlaying)
        {
            ricePouringParticles.Stop();
            isPouring = false;
            Debug.Log("Rice pouring stopped.");
        }
    }
}
