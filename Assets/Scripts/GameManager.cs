using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelHero2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        private GameObject theEndPanel;
        private SaveDataGame saveDataGame;

        private void Awake()
        {
            instance = this;
            theEndPanel = GameObject.Find("The End Panel");
            theEndPanel.SetActive(false);
            saveDataGame = FindObjectOfType<SaveDataGame>();
            LoadGame();
        }

        public void RestartGame()
        {
            ResetPlayerPrefs();
            SceneManager.LoadScene("Prototype");
        }

        private void LoadGame()
        {
            // Obtenemos las preferencias guardadas
            bool success = saveDataGame.LoadPrefs();

            // Establecemos las preferencias iniciales
            if (success)
            {
                setPrefs();
            }
        }

        private void ResetPlayerPrefs()
        {
            saveDataGame.DeletePrefs();
        }

        private void setPrefs()
        {
            // Cargamos los valores iniciales y establecemos la posicion y direccion del player
            PlayerController.instance.transform.localPosition = PlayerController.instance.PlayerPrefs.PlayerLastPosition;
            PlayerController.instance.transform.localScale = PlayerController.instance.PlayerPrefs.PlayerDirection;
            ItemsManager.instance.CoinShiningCounter = PlayerController.instance.PlayerPrefs.CoinsShining;
            ItemsManager.instance.CoinSpinningCounter = PlayerController.instance.PlayerPrefs.CoinsSpining;
            ItemsManager.instance.HeartCounter = PlayerController.instance.PlayerPrefs.Hearts;

            // Ejecutamos el control del desbloqueo de extras con los contadores recuperados
            ItemsManager.instance.UnlockExtras();

            // Eliminamos de la escena los items recogidos
            foreach (string item in PlayerController.instance.PlayerPrefs.ItemsCollected)
            {
                Destroy(GameObject.Find(item));
            }

            // Eliminamos los enemigos liquidados
            foreach (string enemy in PlayerController.instance.PlayerPrefs.EnemiesKilled)
            {
                Destroy(GameObject.Find(enemy));
            }
        }

        public void EndGame()
        {
            Time.timeScale = 0;
            theEndPanel.SetActive(true);
        }

    }
}
