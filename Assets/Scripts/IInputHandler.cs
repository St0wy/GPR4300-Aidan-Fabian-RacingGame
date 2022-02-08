using UnityEngine;

namespace RacingGame
{
    public interface IInputHandler
    {
        public delegate void PauseEventHandler();

        public Vector2 InputVector { get; }
        event PauseEventHandler Pause;
    }
}