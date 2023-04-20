using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelHero2D
{
    public class PlayerController : MonoBehaviour
    {
        // Player Sprites
        private GameObject standingPlayer;
        private GameObject ballPlayer;
        private GameObject climbPlayer;

        [Header("Player Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private LayerMask selectedLayerMask;
        private Rigidbody2D playerRB;
        [SerializeField] private Transform checkGroundPoint;
        [SerializeField] private Transform ledderDetector;
        private Transform transformPlayer;
        private bool isGrounded;
        private bool _isClimbingAllowed;
        private Animator animatorStandingPlayer;
        private Animator animatorBallPlayer;
        private Animator animatorClimb;
        private int IdSpeed, IdIsGrounded, IdShootArrow, IdCanDoubleJump;
        private float ballModeCounter;
        [SerializeField] private float waitForBallMode;
        [SerializeField] private float isGroundedRange;
        private float originalGravityScale;

        [Header("Player Shoot")]
        [SerializeField] private ArrowController arrowController;
        private Transform transformArrowPoint;
        private Transform transformBombPoint;
        [SerializeField] private GameObject prefabBomb;

        [Header("Player Dust")]
        [SerializeField] private GameObject dustJump;
        private Transform transformDustPoint;
        private bool isIdle, canDoubleJump;

        [Header("Player Dash")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashTime;
        private float dashCounter;
        [SerializeField] private float waitForDash;
        private float afterDashCounter;

        [Header("Player Dash After Image")]
        [SerializeField] private SpriteRenderer playerSR;
        [SerializeField] private SpriteRenderer afterImageSR;
        [SerializeField] private float afterImageLifetime;
        [SerializeField] private Color afterImageColor;
        [SerializeField] private float afterImageTimeBetween;
        private float afterImageCounter;

        // Player Extras
        private PlayerExtrasTracker playerExtrasTracker;

        public bool IsClimbingAllowed { get => _isClimbingAllowed; set => _isClimbingAllowed = value; }


        private void Awake()
        {
            playerRB = GetComponent<Rigidbody2D>();
            transformPlayer = GetComponent<Transform>();
            playerExtrasTracker = GetComponent<PlayerExtrasTracker>();
        }

        private void Start()
        {
            standingPlayer = GameObject.Find("StandingPlayer");
            ballPlayer = GameObject.Find("BallPlayer");
            ballPlayer.SetActive(false);
            climbPlayer = GameObject.Find("ClimbPlayer");
            climbPlayer.SetActive(false);
            transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
            transformDustPoint = GameObject.Find("DustPoint").GetComponent<Transform>();
            transformBombPoint = GameObject.Find("BombPoint").GetComponent<Transform>();
            animatorStandingPlayer = standingPlayer.GetComponent<Animator>();
            animatorBallPlayer = ballPlayer.GetComponent<Animator>();
            animatorClimb = climbPlayer.GetComponent<Animator>();
            IdSpeed = Animator.StringToHash("speed");
            IdIsGrounded = Animator.StringToHash("isGrounded");
            IdShootArrow = Animator.StringToHash("shootArrow");
            IdCanDoubleJump = Animator.StringToHash("canDoubleJump");
            originalGravityScale = playerRB.gravityScale;
        }

        void Update()
        {
            Dash();
            Jump();
            CheckAndSetDirection();
            Shoot();
            PlayDust();
            BallMode();
        }

        private void Dash()
        {
            if (afterDashCounter > 0)
            {
                afterDashCounter -= Time.deltaTime;
            }
            else
            {
                if ((Input.GetButtonDown("Fire2") && standingPlayer.activeSelf) && playerExtrasTracker.CanDash)
                {
                    dashCounter = dashTime;
                    ShowAfterImage();
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                playerRB.velocity = new Vector2(dashSpeed * transformPlayer.localScale.x, playerRB.velocity.y);
                afterImageCounter -= Time.deltaTime;

                if (afterImageCounter <= 0)
                {
                    ShowAfterImage();
                }

                afterDashCounter = waitForDash;
            }
            else
            {
                Move();
            }
        }

        private void ShowAfterImage()
        {
            SpriteRenderer afterImage = Instantiate(afterImageSR, transformPlayer.position, transformPlayer.rotation);
            afterImage.sprite = playerSR.sprite;
            afterImage.transform.localScale = transformPlayer.localScale;
            afterImage.color = afterImageColor;
            Destroy(afterImage.gameObject, afterImageLifetime);
            afterImageCounter = afterImageTimeBetween;
        }

        private void Move()
        {
            float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
            float inputY = Input.GetAxisRaw("Vertical") * moveSpeed / 2;

            if (IsClimbingAllowed)
            {
                Climbing(inputX, inputY);
            }
            else
            {
                playerRB.gravityScale = originalGravityScale;
                climbPlayer.SetActive(false);
                standingPlayer.SetActive(true);
                playerRB.velocity = new Vector2(inputX, playerRB.velocity.y);
            }

            if (standingPlayer.activeSelf)
            {
                animatorStandingPlayer.SetFloat(IdSpeed, Mathf.Abs(playerRB.velocity.x));
            }

            if (ballPlayer.activeSelf)
            {
                standingPlayer.SetActive(false);
                animatorBallPlayer.SetFloat(IdSpeed, Mathf.Abs(playerRB.velocity.x));
            }
        }

        private void Climbing(float inputX, float inputY)
        {
            climbPlayer.SetActive(true);
            standingPlayer.SetActive(false);
            playerRB.gravityScale = 0f;
            playerRB.velocity = new Vector2(inputX, inputY);
            animatorClimb.SetFloat(IdSpeed, Mathf.Abs(playerRB.velocity.y));
        }

        private void Jump()
        {
            isGrounded = Physics2D.Raycast(checkGroundPoint.position, Vector2.down, isGroundedRange, selectedLayerMask);
            if ((isGrounded || (canDoubleJump && playerExtrasTracker.CanDoubleJump)) && Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    canDoubleJump = true;
                    Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
                }
                else
                {
                    canDoubleJump = false;
                    animatorStandingPlayer.SetTrigger(IdCanDoubleJump);
                }
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
            animatorStandingPlayer.SetBool(IdIsGrounded, isGrounded);
        }

        private void CheckAndSetDirection()
        {
            if (playerRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (playerRB.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }

        private void Shoot()
        {
            if (Input.GetButtonDown("Fire1") && standingPlayer.activeSelf)
            {
                ArrowController tempArrowController = Instantiate(arrowController, transformArrowPoint.position, transformArrowPoint.rotation);
                tempArrowController.ArrowDirection = new Vector2(transformPlayer.localScale.x, 0f);
                tempArrowController.transform.localScale = new Vector3(transformPlayer.localScale.x, 1, 1);
                animatorStandingPlayer.SetTrigger(IdShootArrow);
            }

            if ((Input.GetButtonDown("Fire1") && ballPlayer.activeSelf) && playerExtrasTracker.CanDropBomb)
            {
                Instantiate(prefabBomb, transformBombPoint.position, Quaternion.identity);
            }
        }

        private void PlayDust()
        {
            if (playerRB.velocity.x != 0 && isIdle && isGrounded)
            {
                isIdle = false;
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
            }

            if (playerRB.velocity.x == 0)
            {
                isIdle = true;
            }
        }

        private void BallMode()
        {
            float inputVertical = Input.GetAxisRaw("Vertical");
            if (((inputVertical <= -0.9f && !ballPlayer.activeSelf) || (inputVertical >= 0.9f && ballPlayer.activeSelf)) && playerExtrasTracker.CanEnterBallMode)
            {
                ballModeCounter -= Time.deltaTime;

                if (ballModeCounter < 0)
                {
                    ballPlayer.SetActive(!ballPlayer.activeSelf);
                    standingPlayer.SetActive(!standingPlayer.activeSelf);
                }
            }
            else
            {
                ballModeCounter = waitForBallMode;
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 sizeLedderDector = new Vector3(0.1f, 0.1f, 0f);
            Gizmos.DrawWireSphere(checkGroundPoint.position, isGroundedRange);
            Gizmos.DrawWireCube(ledderDetector.position, sizeLedderDector);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Enemy") || other.collider.CompareTag("Traps"))
            {
                GameManager.instance.RestartGame();
            }
        }
    }
}
