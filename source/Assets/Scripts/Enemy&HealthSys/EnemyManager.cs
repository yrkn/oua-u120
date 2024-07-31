using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;         // Oyuncunun Transformu
    public float attackRange = 2f;   // Saldırı Mesafesi
    public float chaseRange = 10f;   // Takip Mesafesi
    public float chaseSpeed = 3.5f;  // Takip Hızı
    public float wanderSpeed = 2f;   // Rastgele Dolaşma Hızı
    public float wanderRadius = 10f; // Rastgele Dolaşma Alanı
    public float wanderTimer = 5f;   // Rast. Nokta Belirleme Süresi
    public Animator animator;        // Animator
    public int maxHealth = 100;      // Maksimum Sağlık
    public RuntimeAnimatorController animatorController; // Animator Controller
    public int damageAmount = 20;    // Hasar Miktarı

    private NavMeshAgent navMeshAgent;
    private bool isAttacking = false;
    private float timer;
    
    [SerializeField]
    private int currentHealth;
    private Collider zombieCollider;

    // ÖLEN ZOMBİ SAYISI BURADA TUTULUYOR
    public static int totalZombiesKilled = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        currentHealth = maxHealth;
        zombieCollider = GetComponent<Collider>();

        if (zombieCollider == null)
        {
        }
        if (animatorController == null)
        {
            
        }
        else
        {
            animator.runtimeAnimatorController = animatorController;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            Chase();
        }
        else
        {
            Wander();
        }
    }

    void Chase()
    {
        if (!isAttacking)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
    }

    void Attack()
    {
        navMeshAgent.isStopped = true;
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        // ZOMBİYİ OYUNCUYA ÇEVİRME
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (direction.magnitude > attackRange)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
    }

    void Wander()
    {
        navMeshAgent.speed = wanderSpeed;
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            // RAST KONUM
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        }

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void Idle()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    // SCENE EKRANI - ALAN RENKLERİ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(damageAmount);
            Destroy(other.gameObject); // Mermiyi yok et
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameObject.CompareTag("ZombieTrigger"))
        {
            if (zombieCollider != null)
            {
                zombieCollider.enabled = false;
            }
            
            animator.SetBool("isDead", true); 
            this.enabled = false; 

            totalZombiesKilled++;
            Debug.Log($"{totalZombiesKilled} zombi öldü.");

        }
    }
}
