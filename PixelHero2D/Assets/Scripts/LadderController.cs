using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelHero2D
{
    public class LadderController : MonoBehaviour
    {
        private GameObject player;
        private PlayerController playerController;

        private void Start()
        {
            player = GameObject.Find("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerController.IsClimbingAllowed = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerController.IsClimbingAllowed = false;
            }
        }
    }
}
