using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject Player;
    private Animator animator;

    private bool isDead = false;
    private bool isAttacking = false;

    public float attackDistance = 2f; // Distancia para atacar
    public float attackCooldown = 1.5f; // Tiempo entre ataques

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Verificar si el agente está sobre un NavMesh válido
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("El enemigo no está sobre un NavMesh válido.");
            enabled = false; // Desactivar el script para evitar más errores
        }
    }

    void Update()
    {
        if (isDead || navMeshAgent == null || Player == null) return;

        float distance = Vector3.Distance(Player.transform.position, transform.position);
        animator.SetFloat("Distance", distance);

        if (distance <= attackDistance)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            if (navMeshAgent.isOnNavMesh) // Verificar antes de mover
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(Player.transform.position);
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;

        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true; // Detener al atacar
        }

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);

        // Aplicar daño al jugador si tiene un script de salud
        PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }

        isAttacking = false;

        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = false; // Reanudar movimiento
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("El enemigo ha muerto.");

        if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }

        animator.SetTrigger("Die");

        // Esperar el tiempo de la animación antes de destruir el objeto
        float deathTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        Destroy(gameObject, deathTime);
    }
}
