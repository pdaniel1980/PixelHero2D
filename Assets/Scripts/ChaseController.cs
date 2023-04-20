using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelHero2D
{
    public class ChaseController : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                enemyController.Chase = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                enemyController.Chase = false;
            }
        }
    }
}
