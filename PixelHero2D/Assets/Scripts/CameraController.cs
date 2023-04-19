using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelHero2D
{
    public class CameraController : MonoBehaviour
    {
        private PlayerController playerController;
        private Transform cameraTransform;
        private Transform playerTransform;
        private BoxCollider2D levelLimit;
        private float cameraSizeHorizontal;
        private float cameraSizeVertical;

        void Start()
        {
            levelLimit = GameObject.Find("LevelLimit").GetComponent<BoxCollider2D>();
            cameraTransform = GetComponent<Transform>();
            playerController = FindObjectOfType<PlayerController>();
            playerTransform = playerController.GetComponent<Transform>();
            cameraSizeVertical = Camera.main.orthographicSize;
            cameraSizeHorizontal = Camera.main.orthographicSize * Camera.main.aspect;
        }

        void LateUpdate()
        {
            if (playerController != null)
            {
                cameraTransform.position = new Vector3(Mathf.Clamp(playerTransform.position.x,
                                                                   levelLimit.bounds.min.x + cameraSizeHorizontal,
                                                                   levelLimit.bounds.max.x - cameraSizeHorizontal),
                                                       Mathf.Clamp(playerTransform.position.y,
                                                                   levelLimit.bounds.min.y + cameraSizeVertical,
                                                                   levelLimit.bounds.max.y - cameraSizeVertical),
                                                       cameraTransform.position.z);
            }
        }
    }
}
