using UnityEngine;
using UnityEngine.AI; // Para asegurarnos de que el jefe aparezca en un NavMesh válido

public class BeerPickup : MonoBehaviour
{
    public static int beerCount = 0;  // Contador de cervezas recogidas
    public static int totalBeers = 4; // Total de cervezas en el mapa

    public GameObject bossPrefab; // Prefab del jefe final
    public Transform player; // Referencia al jugador
    public float spawnDistance = 10f; // Distancia mínima de aparición del jefe
    public float maxSpawnDistance = 15f; // Distancia máxima de aparición del jefe

    void Start()
    {
        // Reinicia el contador al cargar la escena
        beerCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador toca la cerveza
        {
            beerCount++;
            Destroy(gameObject); // Destruye la cerveza al recogerla

            if (beerCount >= totalBeers) // Si se recogieron todas las cervezas
            {
                SpawnBoss(); // Llama a la función para spawnear el jefe
            }
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || player == null) return;

        Vector3 spawnPosition = GetValidSpawnPosition();
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("¡Jefe Final Aparece en: " + spawnPosition);
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        int attempts = 10; // Intentos para encontrar una posición válida

        for (int i = 0; i < attempts; i++)
        {
            // Genera un punto en un círculo alrededor del jugador
            Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(spawnDistance, maxSpawnDistance);
            spawnPosition = player.position + new Vector3(randomPoint.x, 0, randomPoint.y);

            // Ajusta la posición para que esté en el NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 2f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                validPositionFound = true;
                break;
            }
        }

        // Si después de los intentos no encuentra una posición válida, usa la posición original del jugador
        return validPositionFound ? spawnPosition : player.position + new Vector3(spawnDistance, 0, 0);
    }
}
