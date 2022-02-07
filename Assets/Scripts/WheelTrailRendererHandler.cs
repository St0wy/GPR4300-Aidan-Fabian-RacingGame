using System;
using UnityEngine;

namespace RacingGame
{
    /// <summary>
    /// https://youtu.be/PpYTNc7B8A8
    /// </summary>
    [RequireComponent(typeof(TrailRenderer))]
    public class WheelTrailRendererHandler : MonoBehaviour
    {
        private CarController carController;
        private TrailRenderer trailRenderer;

        private void Awake()
        {
            carController = GetComponentInParent<CarController>();
            trailRenderer = GetComponent<TrailRenderer>();

            trailRenderer.emitting = false;
        }

        private void Update()
        {
            //If the car tires are screeching then we'll emit a trail.
            trailRenderer.emitting = carController.IsTireScreeching;
        }
    }
}