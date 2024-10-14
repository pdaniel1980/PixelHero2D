using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace PixelHero2D
{
    public class SaveDataGame : MonoBehaviour
    {
        private string fileName = "/saveGame.dat";
        private string playerPrefsTag = "PlayerPrefs";

        byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        byte[] iVector = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        [SerializeField] private PlayerPrefs playerPrefs;

        private void Awake()
        {
            playerPrefs = FindObjectOfType<PlayerPrefs>();
        }

        private void OnApplicationQuit()
        {
            JObject jDataSave = new JObject();
            
            playerPrefs.PlayerLastPosition = playerPrefs.GetComponentInParent<PlayerController>().transform.position;
            playerPrefs.PlayerDirection = playerPrefs.GetComponentInParent<PlayerController>().transform.localScale;
            playerPrefs.CoinsShining = ItemsManager.instance.CoinShiningCounter;
            playerPrefs.CoinsSpining = ItemsManager.instance.CoinSpinningCounter;
            playerPrefs.Hearts = ItemsManager.instance.HeartCounter;
            
            JObject serializePlayerPrefs = playerPrefs.Serialize();
            jDataSave.Add(playerPrefsTag, serializePlayerPrefs);

            SavePrefs(jDataSave.ToString());

        }

        private void SavePrefs(string dataSave)
        {
            string filePath = Application.persistentDataPath + fileName;
            byte[] encryptedSaveGame = Encrypt(dataSave);
            File.WriteAllBytes(filePath, encryptedSaveGame);
        }

        public void LoadPrefs()
        { 
            string filePath = Application.persistentDataPath + fileName;
            byte[] decryptedSaveGame = File.ReadAllBytes(filePath);
            string jsonString = Decrypt(decryptedSaveGame);

            JObject jsonPrefs = JObject.Parse(jsonString);

            playerPrefs.DeSerialized(jsonPrefs[playerPrefsTag].ToString());

            if (playerPrefs.ItemsCollected.Count > 0)
            {
                Debug.Log("SaveGame Path: " + filePath);
            }
        }

        byte[] Encrypt(string plainText)
        {
            AesManaged aesManaged = new AesManaged();
            ICryptoTransform cryptoTransform = aesManaged.CreateEncryptor(key, iVector);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(plainText);

            streamWriter.Close();
            cryptoStream.Close();
            memoryStream.Close();

            return memoryStream.ToArray();
        }

        string Decrypt(byte[] encryptedText)
        {
            AesManaged aesManaged = new AesManaged();
            ICryptoTransform decryptTransform = aesManaged.CreateDecryptor(key, iVector);
            MemoryStream memoryStream = new MemoryStream(encryptedText);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptTransform, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);

            string decryptedPlainText = streamReader.ReadToEnd();
            streamReader.Close();
            cryptoStream.Close();
            memoryStream.Close();

            return decryptedPlainText;
        }

    }
}
