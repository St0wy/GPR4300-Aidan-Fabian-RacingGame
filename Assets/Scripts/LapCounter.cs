using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RacingGame
{
    public class LapCounter : MonoBehaviour
    {
        [SerializeField] private GameObject[] checkPoints;
        [SerializeField] private Text winText;
        [SerializeField] private Timer timer;
        
        private CarSfxHandler carSfxHandler;

        private bool[] lapPassed;

        private void Awake()
        {
            lapPassed = new bool[checkPoints.Length - 1];
            carSfxHandler = GetComponent<CarSfxHandler>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            for (var i = 0; i < checkPoints.Length; i++)
            {
                // Check that we collide with a checkpoint
                if (checkPoints[i] != col.gameObject) continue;

                if (i < checkPoints.Length - 1 && !lapPassed[i])
                {
                    // Passed a middle checkpoint
                    lapPassed[i] = true;
                }
                // Check if we passed all the middle checkpoint
                else if (lapPassed.All(x => x == true))
                {
                    // Stop the timer
                    timer.StopTimer();
                    carSfxHandler.engine.Stop();
                    carSfxHandler.tiresScreeching.Stop();
                    ShowWinScreen();
                }
            }
        }

        private void ShowWinScreen()
        {
            timer.gameObject.SetActive(false);
            winText.gameObject.SetActive(true);
            winText.text = $"You finished in : {timer}";
            Time.timeScale = 0f;
        }
    }
}