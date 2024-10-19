using UnityEngine;

namespace PixelHero2D
{
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private GameObject arrowImpact;
        [SerializeField] private float arrowSpeed;

        private Rigidbody2D arrowRB;
        private Vector2 _arrowDirection;

        public Vector2 ArrowDirection { get => _arrowDirection; set => _arrowDirection = value; }

        private Transform transformArrow;

        private void Awake()
        {
            arrowRB = GetComponent<Rigidbody2D>();
            transformArrow = GetComponent<Transform>();
        }

        void Update()
        {
            arrowRB.velocity = _arrowDirection * arrowSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController.instance.DestroyEnemy(other);
                Destroy(gameObject);
            }
            else if (!isItem(other.gameObject.tag))
            {
                Instantiate(arrowImpact, transformArrow.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        private bool isItem(string tag)
        {
            for (int i = 0; i < ItemsManager.itemsTag.Length; i++)
            {
                if (tag == ItemsManager.itemsTag[i])
                    return true;
            }

            return false;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
