using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo a spawnear
    public Transform player; // Referencia al jugador
    public float spawnRadius = 10f; // Distancia máxima al jugador
    public float spawnInterval = 5f; // Tiempo entre spawns

    private void Start()
    {

        InvokeRepeating("SpawnEnemy", 2f, spawnInterval); // Llama a SpawnEnemy cada X segundos
    }


    void SpawnEnemy()
    {
        if (player == null) return; // Evitar errores si el jugador no está asignado

        // Calcular posición aleatoria alrededor del jugador
        Vector3 spawnPosition = player.position + (Random.insideUnitSphere * spawnRadius);
        spawnPosition.y = player.position.y; // Mantener la altura del jugador

        // Instanciar enemigo en la posición generada
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
