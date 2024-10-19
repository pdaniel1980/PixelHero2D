using UnityEngine;

namespace PixelHero2D
{
    public class ItemsManager : MonoBehaviour
    {
        public static ItemsManager instance;
        private PlayerExtrasTracker playerExtrasTracker;

        private int _heartCounter, _coinShiningCounter, _coinSpinningCounter;

        public int HeartCounter { get => _heartCounter; set => _heartCounter = value; }
        public int CoinShiningCounter { get => _coinShiningCounter; set => _coinShiningCounter = value;  }
        public int CoinSpinningCounter { get => _coinSpinningCounter; set => _coinSpinningCounter = value; }

        [Header("Extras Settings")]
        [SerializeField] private int coinShiningRequire = 10;
        [SerializeField] private int coinSpinningRequire = 6;
        [SerializeField] private int heartRequire = 5;

        public static string[] itemsTag = { "ItemCoinShining", "ItemCoinSpinning", "ItemHeart", "ItemTreasure" };

        public enum Items { CoinShining, CoinSpinning, Heart }

        private void Awake()
        {
            instance = this;
            playerExtrasTracker = GameObject.Find("Player").GetComponent<PlayerExtrasTracker>();
        }

        public void CollectItem(Transform transformItem)
        {
            if (transformItem.CompareTag("ItemCoinShining"))
            {
                _coinShiningCounter += 1;
            }
            else if (transformItem.CompareTag("ItemCoinSpinning"))
            {
                _coinSpinningCounter += 1;
            }
            else if (transformItem.CompareTag("ItemHeart"))
            {
                _heartCounter += 1;
            }

            UnlockExtras();
        }

        public void UnlockExtras()
        {
            if (HeartCounter == heartRequire)
            {
                playerExtrasTracker.CanDoubleJump = true;
            }

            if (CoinSpinningCounter == coinSpinningRequire)
            {
                playerExtrasTracker.CanDash = true;
            }

            if (CoinShiningCounter == coinShiningRequire)
            {
                playerExtrasTracker.CanEnterBallMode = true;
                playerExtrasTracker.CanDropBomb = true;
            }
        }

        public int RemainingItem(Items type)
        {
            int remaining = 99;

            switch (type)
            {
                case Items.CoinShining:
                    {
                        remaining = coinShiningRequire - CoinShiningCounter;
                        break;
                    }
                case Items.CoinSpinning:
                    {
                        remaining = coinSpinningRequire - CoinSpinningCounter;
                        break;
                    }
                case Items.Heart:
                    {
                        remaining = heartRequire - HeartCounter;
                        break;
                    }

            }

            return remaining >= 0 ? remaining : 0;
        }

    }
}
