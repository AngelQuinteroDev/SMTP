using UnityEngine;

public class Curar : MonoBehaviour
{
    public int healAmount = 20; // Cantidad de vida a recuperar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene el tag "Player"
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject); // Destruir el objeto después de recogerlo
            }
        }
    }
}
