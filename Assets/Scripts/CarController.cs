using UnityEngine;

namespace RacingGame
{
    /// <summary>
    /// Controller for a top down car.
    /// Inspired by : https://www.youtube.com/watch?v=DVHcOS1E5OQ
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CarController : MonoBehaviour
    {
        #region Fields

        [Header("Car settings")] [SerializeField]
        private float driftFactor = 0.95f;

        [SerializeField] private float accelerationFactor = 30f;
        [SerializeField] private float turnFactor = 3.5f;
        [SerializeField] private float maxSpeed = 20f;

        private float accelerationInput = 0f;
        private float steeringInput = 0f;
        private float rotationAngle = 0f;
        private float velocityVsUp = 0f;

        private Rigidbody2D carRigidbody;
        private IInputHandler inputHandler;

        #endregion

        #region Properties

        public bool IsBraking => accelerationInput < 0;

        /// <summary>
        /// Gets how fast the car is moving sideways.
        /// </summary>
        /// <returns>The lateral velocity.</returns>
        public float LateralVelocity => Vector2.Dot(transform.right, carRigidbody.velocity);

        public bool IsTireScreeching
        {
            get
            {
                // Check if we are moving forward and if the player is hitting the brakes. In that case the tires should screech.
                if (IsBraking && velocityVsUp > 0)
                    return true;

                // If we have a lot of side movement then the tires should be screeching
                return Mathf.Abs(LateralVelocity) > 4.0f;
            }
        }

        public float VelocityMagnitude => carRigidbody.velocity.magnitude;

        #endregion

        #region Methods

        private void Awake()
        {
            carRigidbody = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<IInputHandler>();
        }

        private void FixedUpdate()
        {
            // Read the input vector
            var inputVector = inputHandler.InputVector;
            accelerationInput = inputVector.y;
            steeringInput = inputVector.x;

            // Apply the movement
            ApplyEngineForce();
            KillOrthogonalVelocity();
            ApplySteering();
        }

        private void ApplyEngineForce()
        {
            // Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
            if (accelerationInput == 0)
                carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 3.0f, Time.fixedDeltaTime * 3);
            else carRigidbody.drag = 0;

            // Calculate how much "forward" we are going in terms of the direction of our velocity
            velocityVsUp = Vector2.Dot(transform.up, carRigidbody.velocity);

            // Limit so we cannot go faster than the max speed in the "forward" direction
            if (velocityVsUp > maxSpeed && accelerationInput > 0)
                return;

            // Limit so we cannot go faster than the 50% of max speed in the "reverse" direction
            if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
                return;

            // Limit so we cannot go faster in any direction while accelerating
            if (carRigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
                return;

            // Create a force for the engine
            Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

            // Apply force and pushes the car forward
            carRigidbody.AddForce(engineForceVector, ForceMode2D.Force);
        }

        private void KillOrthogonalVelocity()
        {
            // Get forward and right velocity of the car
            var localTransform = transform;
            var up = localTransform.up;
            var right = localTransform.right;
            Vector2 forwardVelocity = up * Vector2.Dot(carRigidbody.velocity, up);
            Vector2 rightVelocity = right * Vector2.Dot(carRigidbody.velocity, right);

            // Kill the orthogonal velocity (side velocity) based on how much the car should drift. 
            carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
        }

        private void ApplySteering()
        {
            // Limit the cars ability to turn when moving slowly
            var minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(carRigidbody.velocity.magnitude / 2f);

            // Update the rotation angle based on input
            rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

            // Apply steering by rotating the car object
            carRigidbody.MoveRotation(rotationAngle);
        }

        #endregion
    }
}