using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RacingGame
{
    public class PauseMenuBehaviour : MonoBehaviour
    {
        public static bool IsPaused = false;

        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private Button resumeButton;
        [SerializeField] private GameObject car;

        [SerializeField] private GameObject winText;

        private IInputHandler inputHandler;

        private void Awake()
        {
            inputHandler = car.GetComponent<IInputHandler>();
            inputHandler.Pause += OnPause;
        }

        [UsedImplicitly]
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            IsPaused = false;
            SceneManager.LoadScene(0);
        }

        [UsedImplicitly]
        public void QuitGame()
        {
            Application.Quit();
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            resumeButton.Select();
            Time.timeScale = 0f;
            IsPaused = true;

            if (winText.activeSelf)
            {
                resumeButton.gameObject.SetActive(false);
                winText.SetActive(false);
            }
        }

        [UsedImplicitly]
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            resumeButton.Select();
            Time.timeScale = 1f;
            IsPaused = false;
        }

        [UsedImplicitly]
        private void OnPause()
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}