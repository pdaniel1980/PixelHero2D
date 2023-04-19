using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelHero2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        private GameObject theEndPanel;

        private void Awake()
        {
            instance = this;
            theEndPanel = GameObject.Find("The End Panel");
            theEndPanel.SetActive(false);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Prototype");
        }

        public void EndGame()
        {
            Time.timeScale = 0;
            theEndPanel.SetActive(true);
        }

    }
}
