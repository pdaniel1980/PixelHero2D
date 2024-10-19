using UnityEngine;

namespace PixelHero2D
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private int startingPoint = 0;
        [SerializeField] private Transform[] transformPoints;
        private Transform transformPlatform;

        private int i;

        private void Awake()
        {
            transformPlatform = GetComponent<Transform>();
            transformPlatform.position = transformPoints[startingPoint].position;
        }

        private void Update()
        {
            if (Vector2.Distance(transformPlatform.position, transformPoints[i].position) < 0.2f)
            {
                i++;
                if (i == transformPoints.Length)
                {
                    i = 0;
                }
            }

            transformPlatform.position = Vector2.MoveTowards(transformPlatform.position, transformPoints[i].position, moveSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            other.transform.SetParent(transformPlatform);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            other.transform.SetParent(null);
        }
    }
}
