using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace RacingGame
{
    public class Timer : MonoBehaviour
    {
        private Text timerText;
        private Stopwatch sw;

        public double ElapsedMilliseconds => sw.ElapsedMilliseconds;

        private void Awake()
        {
            timerText = GetComponent<Text>();
            sw = new Stopwatch();
        }

        private void Start()
        {
            StartTimer();
        }

        private void Update()
        {
            timerText.text = sw.IsRunning ? ToString() : string.Empty;
        }

        public void StartTimer()
        {
            sw.Start();
        }

        public void StopTimer()
        {
            sw.Stop();
        }

        public override string ToString()
        {
            double elapsed = sw.ElapsedMilliseconds;
            var seconds = (int) (elapsed / 1000 % 60);
            var minutes = seconds / 60;
            return $"{minutes:00}:{seconds:00}";
        }
    }
}