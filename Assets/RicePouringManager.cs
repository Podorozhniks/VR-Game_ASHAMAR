using UnityEngine;

public class RicePouringManager : MonoBehaviour
{
    public ParticleSystem ricePouringParticles;   
    public Transform bowlTransform;               
    public float pouringAngleThreshold = 45f;     
    private bool isPouring = false;               

    void Update()
    {
        
        float tiltAngle = Vector3.Angle(bowlTransform.up, Vector3.up);

       
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
