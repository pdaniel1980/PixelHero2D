using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelHero2D
{
    public class FrameRateController : MonoBehaviour
    {
        [SerializeField] private int target;

        private void Awake()
        {
            Application.targetFrameRate = target;
        }
    }
}
