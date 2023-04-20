using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelHero2D
{
    public class BombController : MonoBehaviour
    {
        [SerializeField] private float waitForExplode;
        [SerializeField] private float waitForDestroy;
        private Animator animator;
        private bool isActive;
        private int IdIsActive;
        [SerializeField] private Transform transformBomb;
        [SerializeField] private float expansiveWaveRange;
        [SerializeField] private LayerMask isDetroyable;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            IdIsActive = Animator.StringToHash("isActive");
            transformBomb = GetComponent<Transform>();
        }

        private void Update()
        {
            waitForExplode -= Time.deltaTime;
            waitForDestroy -= Time.deltaTime;

            if (waitForExplode <= 0 && !isActive)
            {
                ActivateBomb();
            }

            if (waitForDestroy <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void ActivateBomb()
        {
            isActive = true;
            animator.SetBool(IdIsActive, isActive);
            Collider2D[] destroyedObjects = Physics2D.OverlapCircleAll(transformBomb.position, expansiveWaveRange, isDetroyable);
            if (destroyedObjects.Length > 0)
            {
                foreach (var col in destroyedObjects)
                {
                    Destroy(col.gameObject);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transformBomb.position, expansiveWaveRange);
        }
    }
}
