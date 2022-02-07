using UnityEngine;
using UnityEngine.SceneManagement;

namespace RacingGame
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public const int GameSceneId = 1;
        
        public void StartGame()
        {
            SceneManager.LoadScene(GameSceneId);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}