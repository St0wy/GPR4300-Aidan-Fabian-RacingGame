using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RacingGame
{
    public class CarInputHandler : MonoBehaviour, IInputHandler
    {
        public Vector2 InputVector { get; private set; }
        public event IInputHandler.PauseEventHandler Pause;

        [UsedImplicitly]
        private void OnMove(InputValue value)
        {
            InputVector = value.Get<Vector2>();
        }

        [UsedImplicitly]
        private void OnPause()
        {
            Pause?.Invoke();
        }
    }
}