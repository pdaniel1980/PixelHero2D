using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PixelHero2D
{
    public class PlayerPrefs : MonoBehaviour
    {
        [SerializeField] private int coinsShining;
        [SerializeField] private int coinsSpining;
        [SerializeField] private int hearts;
        [SerializeField] private Vector3 playerDirection;
        [SerializeField] private Vector3 playerStartPosition;
        [SerializeField] private Vector3 playerLastPosition;
        [SerializeField] private List<string> itemsCollected;
        [SerializeField] private List<string> enemiesKilled;

        public int CoinsShining { get => coinsShining; set => coinsShining = value; }
        public int CoinsSpining { get => coinsSpining; set => coinsSpining = value; }
        public int Hearts { get => hearts; set => hearts = value; }
        public Vector3 PlayerDirection { get => playerDirection; set => playerDirection = value; }
        public Vector3 PlayerStartPosition { get => playerStartPosition; set => playerStartPosition = value; }
        public Vector3 PlayerLastPosition { get => playerLastPosition; set => playerLastPosition = value; }
        public List<string> ItemsCollected { get => itemsCollected; set => itemsCollected = value; }
        public List<string> EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }

        public JObject Serialize()
        {
            string jsonString = JsonUtility.ToJson(this);
            JObject returnObject = JObject.Parse(jsonString);
            return returnObject;
        }
        public void DeSerialized(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }
}
