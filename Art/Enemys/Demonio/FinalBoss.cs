using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FinalBoss : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject Player;
    private Animator animator;

    private bool isDead = false;
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool canJump = true; // Controla el cooldown del salto

    public float attackDistance = 2f;
    public float minJumpDistance = 3f; // Distancia mínima para saltar
    public float maxJumpDistance = 5f; // Distancia máxima para saltar
    public float attackCooldown = 1.5f;
    public float jumpCooldown = 15f; // Cooldown ajustado a 15s
    public float jumpSpeed = 10f;
    public float jumpDamage = 25f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("El jefe no está sobre un NavMesh válido.");
            enabled = false;
        }
    }

    void Update()
    {
        if (isDead || isJumping || Player == null) return;


        float distance = Vector3.Distance(Player.transform.position, transform.position);
        animator.SetFloat("Distance", distance);

        if (distance <= attackDistance)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else if (distance >= minJumpDistance && distance <= maxJumpDistance && canJump) // Salta si está entre 3 y 5 unidades de distancia
        {
            StartCoroutine(JumpAttack());
        }
        else
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(Player.transform.position);
            }
        }


        ///////////////////////////////////////////////
        if (distance <= attackDistance && !isAttacking && !isJumping)
        {
            StartCoroutine(AttackPlayer());
        }

        ///////////////////////////////////////////////

    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        navMeshAgent.isStopped = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); // Ajustar el tiempo para que coincida con el golpe de la animación

        PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }

        yield return new WaitForSeconds(attackCooldown); // Esperar el cooldown

        isAttacking = false;
        navMeshAgent.isStopped = false;
    }


    IEnumerator JumpAttack()
    {
        isJumping = true;
        canJump = false; // Bloquea el salto hasta que pase el cooldown
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        animator.SetTrigger("Jump");

        Vector3 jumpTarget = Player.transform.position;
        Vector3 startPos = transform.position;
        float jumpTime = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < jumpTime)
        {
            transform.position = Vector3.Lerp(startPos, jumpTarget, elapsedTime / jumpTime);
            elapsedTime += Time.deltaTime * jumpSpeed;
            yield return null;
        }

        // Asegurar que el jefe aterrice correctamente en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        // Verifica si el jugador está dentro del área de daño después del salto
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= 2f)
        {
            PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(jumpDamage);
            }
        }

        yield return new WaitForSeconds(0.5f); // Pausa breve después de aterrizar

        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false;
        isJumping = false;

        // **Inmediatamente reanuda la persecución después del salto**
        if (!isDead && Player != null)
        {
            navMeshAgent.SetDestination(Player.transform.position);
        }

        // Espera el cooldown antes de permitir otro salto
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;

        animator.SetTrigger("Walk");

    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("El jefe ha muerto.");
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        animator.SetTrigger("Die");

        float deathTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Destroy(gameObject, deathTime);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("YouWin");
    }

}
