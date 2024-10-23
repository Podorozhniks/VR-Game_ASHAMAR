using UnityEngine;

public class LiquidStayOnSurface : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public LayerMask surfaceLayer;

    void OnParticleCollision(GameObject other)
    {
        if (((1 << other.layer) & surfaceLayer) != 0)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
            int numParticlesAlive = particleSystem.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                // Stop the particle if it collides with the surface
                particles[i].velocity = Vector3.zero;
                particles[i].remainingLifetime = Mathf.Infinity; // Optional, let particles "stay"
            }

            particleSystem.SetParticles(particles, numParticlesAlive);
        }
    }
}
