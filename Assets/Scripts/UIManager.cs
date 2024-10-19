using UnityEngine;

namespace PixelHero2D
{
    public class UIManager : MonoBehaviour
    {
        private void OnGUI()
        {
            ItemsManager itemsManager = ItemsManager.instance;

            GUI.color = Color.white;
            GUI.skin.label.fontSize = 40;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUILayout.Label("Remaining Items");            
            GUILayout.Label("Coin Shining: " + itemsManager.RemainingItem(ItemsManager.Items.CoinShining));
            GUILayout.Label("Coin Spinning: " + itemsManager.RemainingItem(ItemsManager.Items.CoinSpinning));
            GUILayout.Label("Heart: " + itemsManager.RemainingItem(ItemsManager.Items.Heart));

            // Inicializa el estilo
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

            // Configurar el color del fondo y del texto
            //buttonStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f)); // Fondo gris
            buttonStyle.normal.textColor = Color.white; // Texto blanco

            // Opcional: Configura otros aspectos del estilo, como el tamaño de la fuente
            buttonStyle.fontSize = 30; // Cambiar tamaño de la letra

            if (GUI.Button(new Rect(Screen.width - 270, 5, 260, 90), "RESTART GAME", buttonStyle))
            {
                GameManager.instance.RestartGame();
            }
        }
    }
}
