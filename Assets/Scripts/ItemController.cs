using System.Collections;
using UnityEngine;

namespace PixelHero2D
{
    public class ItemController : MonoBehaviour
    {
        private Transform transformItem;
        [SerializeField] private float timeToReachTarget = 0.5f;
        [SerializeField] private float rangeFadeDistance = 2f;

        private Vector3 startPosition;
        private Vector3 target;
        private bool isCollected;

        private void Awake()
        {
            transformItem = GetComponent<Transform>();
        }

        private void Start()
        {
            startPosition = transformItem.position;
            target = new Vector3(transformItem.position.x, transformItem.position.y + rangeFadeDistance, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && transformItem.CompareTag("ItemTreasure"))
            {
                GameManager.instance.EndGame();
            }
            else if (other.CompareTag("Player") && !isCollected)
            {
                isCollected = true;
                PlayerController.instance.PlayerPrefs.ItemsCollected.Add(transform.name);
                _ = StartCoroutine(DisappearItem());
                ItemsManager.instance.CollectItem(transformItem);
            }
        }

        IEnumerator DisappearItem()
        {
            var itemSR = transformItem.GetComponent<SpriteRenderer>();
            float alpha = itemSR.material.color.a;

            for (float t = 1.0f; t > 0f; t -= Time.deltaTime / timeToReachTarget)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(0f, alpha, t));
                itemSR.material.color = newColor;
                transformItem.position = Vector3.Lerp(startPosition, target, 1f - t);

                yield return null;
            }

            Destroy(transformItem.gameObject);
        }

    }
}
