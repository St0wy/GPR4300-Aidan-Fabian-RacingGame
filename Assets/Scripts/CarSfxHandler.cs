using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RacingGame
{
    public class CarSfxHandler : MonoBehaviour
    {
        [Header("Audio Sources")] public AudioSource tiresScreeching;
        public AudioSource engine;
        public AudioSource carHit;

        private float desiredEnginePitch = 0.5f;
        private float tireScreechPitch = 0.5f;

        private CarController carController;

        private void Awake()
        {
            carController = GetComponentInParent<CarController>();
        }

        private void Update()
        {
            UpdateEngineSfx();
            UpdateTiresScreechingSfx();
        }

        private void UpdateTiresScreechingSfx()
        {
            // Handle tire screeching SFX
            if (carController.IsTireScreeching)
            {
                //If the car is braking we want the tire screech to be louder and also change the pitch.
                if (carController.IsBraking)
                {
                    tiresScreeching.volume = Mathf.Lerp(tiresScreeching.volume, 1.0f, Time.deltaTime * 10f);
                    tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10f);
                }
                else
                {
                    //If we are not braking we still want to play this screech sound if the player is drifting.
                    tiresScreeching.volume = Mathf.Abs(carController.LateralVelocity) * 0.05f;
                    tireScreechPitch = Mathf.Abs(carController.LateralVelocity) * 0.1f;
                }
            }
            else
            {
                //Fade out the tire screech SFX if we are not screeching. 
                tiresScreeching.volume = Mathf.Lerp(tiresScreeching.volume, 0, Time.deltaTime * 10f);
            }
        }

        private void UpdateEngineSfx()
        {
            // Handle engine Sfx
            var velocityMagnitude = carController.VelocityMagnitude;

            // Increase the engine volume as the car goes faster
            var desiredVolume = velocityMagnitude * 0.05f;

            // Clamp the volume at a max level
            desiredVolume = Mathf.Clamp(desiredVolume, 0.2f, 1.0f);

            engine.volume = Mathf.Lerp(engine.volume, desiredVolume, Time.deltaTime * 10f);

            //To add more variation to the engine sound we also change the pitch
            desiredEnginePitch = velocityMagnitude * 0.2f;
            desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
            engine.pitch = Mathf.Lerp(engine.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            //Get the relative velocity of the collision
            var relativeVelocity = col.relativeVelocity.magnitude;

            var volume = relativeVelocity * 0.1f;

            carHit.pitch = Random.Range(0.95f, 1.05f);
            carHit.volume = volume;

            if (!carHit.isPlaying)
                carHit.Play();
        }
    }
}