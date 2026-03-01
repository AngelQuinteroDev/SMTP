using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Para recargar la escena

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthBar; // Arrastra el Slider aquí en el inspector

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount) // Cambié int por float
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.value = currentHealth; // Asegurar que la barra se actualice
        Debug.Log("Vida recuperada: " + currentHealth);
    }

    void Die()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver"); // Asegúrate de que la escena "GameOver" existe
    }
}
