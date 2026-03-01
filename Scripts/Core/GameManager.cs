using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton para acceso global

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para cambiar de escena
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Método para reiniciar el juego (desde Game Over)
    public void RestartGame()
    {
        SceneManager.LoadScene("Game"); // Cambia "Juego" por el nombre real de tu escena de juego
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Application.Quit();
    }
}
