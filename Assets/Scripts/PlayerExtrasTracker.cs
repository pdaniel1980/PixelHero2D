using UnityEngine;

namespace PixelHero2D
{
    public class PlayerExtrasTracker : MonoBehaviour
    {
        [SerializeField] private bool _canDoubleJump, _canDash, _canEnterBallMode, _canDropBomb;

        public bool CanDoubleJump { get => _canDoubleJump; set => _canDoubleJump = value; }
        public bool CanDash { get => _canDash; set => _canDash = value; }
        public bool CanEnterBallMode { get => _canEnterBallMode; set => _canEnterBallMode = value; }
        public bool CanDropBomb { get => _canDropBomb; set => _canDropBomb = value; }
    }
}
