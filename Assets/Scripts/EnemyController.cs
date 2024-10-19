using UnityEngine;

namespace PixelHero2D
{
    public class EnemyController : MonoBehaviour
    {
        public static EnemyController instance;

        [Header("Properties")]
        [SerializeField] GameObject enemyImpact;
        [SerializeField] private float moveSpeed;
        private enum EnemyType { Patrol, Hunter }
        [SerializeField] private EnemyType enemyType;

        [Header("Landmarks")]
        [SerializeField] private Transform[] transformsFlyingPoint;
        private int startingPoint = 0;
        private int i;
        private Vector2 startPosition;
        private bool leftDirection;

        private Rigidbody2D enemyRB;
        private SpriteRenderer enemySR;
        private Transform transformEnemy;
        
        private Animator animatorBatSleeping;
        private int IdIsSleep;

        private GameObject player;

        // Chase Enemy
        private bool _chase;
        public bool Chase { get => _chase; set => _chase = value; }

        private void Awake()
        {
            instance = this;
            enemyRB = GetComponent<Rigidbody2D>();
            enemySR = GetComponent<SpriteRenderer>();
            transformEnemy = GetComponent<Transform>();
            if (EnemyType.Patrol == enemyType)
            {
                transformEnemy.position = transformsFlyingPoint[startingPoint].position;
            }
            
        }

        void Start()
        {
            startPosition = enemyRB.position;
            animatorBatSleeping = GetComponent<Animator>();
            IdIsSleep = Animator.StringToHash("isSleep");
            animatorBatSleeping.SetBool(IdIsSleep, (enemyType == EnemyType.Hunter));
            player = GameObject.Find("Player");
        }


        void Update()
        {
            if (enemyType == EnemyType.Patrol)
            {
                animatorBatSleeping.SetBool(IdIsSleep, false);
                HorizontalRandomMovement();
            }

            if (enemyType == EnemyType.Hunter)
            {
                CheckChasingMode();
            }
        }

        private void HorizontalRandomMovement()
        {   
            if (Vector2.Distance(transformEnemy.position, transformsFlyingPoint[i].position) < 0.2f)
            {
                if (!leftDirection)
                {
                    i++;
                }
                else
                {
                    i--;
                }

                if (i == transformsFlyingPoint.Length)
                {
                    i -= 1;
                    leftDirection = true;
                    enemySR.flipX = true;
                }

                if (i == 0)
                {
                    enemySR.flipX = false;
                    leftDirection = false;
                }
            }

            transformEnemy.position = Vector2.MoveTowards(transformEnemy.position, transformsFlyingPoint[i].position, moveSpeed * Time.deltaTime);
        }

        private void CheckChasingMode()
        {
            if (Chase)
            {
                ChasePlayer();
            }
            else
            {
                BackToStartPosition();
            }
        }

        private void ChasePlayer()
        {
            animatorBatSleeping.SetBool(IdIsSleep, false);
            transformEnemy.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        private void BackToStartPosition()
        {
            transformEnemy.position = Vector2.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(startPosition, transformEnemy.position) == 0f)
                animatorBatSleeping.SetBool(IdIsSleep, true);
        }

        public void DestroyEnemy(Collider2D other)
        {
            PlayerController.instance.PlayerPrefs.EnemiesKilled.Add(other.transform.parent.name);
            Instantiate(enemyImpact, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }

    }
}
