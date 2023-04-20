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
        }
    }
}
