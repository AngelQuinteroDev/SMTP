using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onDeath;

    private Animator animator; // 🎭 Referencia al Animator
    private bool isDead = false; // Para evitar que muera varias veces

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Obtiene el Animator del enemigo
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; // Evita que se le haga daño después de muerto

        currentHealth -= damageAmount;
        onDamage?.Invoke();

        Debug.Log($"{gameObject.name} recibió {damageAmount} de daño. Salud actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} ha muerto.");
        onDeath?.Invoke();

        // Llama a Die() del script Enemy
        GetComponent<FinalBoss>()?.Die();
        GetComponent<Enemy>()?.Die();
    }
}
