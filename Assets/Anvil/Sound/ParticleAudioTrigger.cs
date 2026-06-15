using UnityEngine;

namespace Anvil
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAudioTrigger : AudioTrigger
    {
        private ParticleSystem ps;
        private ParticleSystem.Particle[] particles;
        private int previousFrameCount = 0;

        void Start()
        {
            ps = GetComponent<ParticleSystem>();
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
        }

        void Update()
        {
            int currentParticleCount = ps.GetParticles(particles);

            // Detect Spawns
            if (currentParticleCount > previousFrameCount)
            {
                Trigger(onEnable);
            }
        
            // Detect Deaths
            // This gets tricky if particles are spawning AND dying in the same frame.
            if (currentParticleCount < previousFrameCount)
            {
                Trigger(onDisable);
            }

            previousFrameCount = currentParticleCount;
        }
    }
}