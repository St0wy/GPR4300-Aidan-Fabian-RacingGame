using System;
using UnityEngine;

namespace RacingGame
{
    public class WheelParticleHandler : MonoBehaviour
    {
        public const float DefaultParticleEmissionRate = 30f;

        private float particleEmissionRate = 0;

        private CarController carController;
        private ParticleSystem particleSystem;
        private ParticleSystem.EmissionModule particleEmissionModule;

        private void Awake()
        {
            // Get the components
            carController = GetComponentInParent<CarController>();
            particleSystem = GetComponent<ParticleSystem>();
            particleEmissionModule = particleSystem.emission;

            // Set the emission to 0
            particleEmissionModule.rateOverTime = 0;
        }

        private void Update()
        {
            // Reduce the particles over time
            particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
            particleEmissionModule.rateOverTime = particleEmissionRate;

            // Don't continue if the tires are not screeching
            if (!carController.IsTireScreeching) return;

            // If the car tires are screeching then we'll emit smoke. If the player is braking then emit a lot of smoke.
            if (carController.IsBraking)
                particleEmissionRate = DefaultParticleEmissionRate;
            else
                // If the player is drifting we'll emit smoke based on how much the player is drifting. 
                particleEmissionRate = Mathf.Abs(carController.LateralVelocity) * 2;
        }
    }
}