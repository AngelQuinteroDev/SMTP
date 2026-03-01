using UnityEngine;
using UnityEngine.UIElements;

public class KeyManager : MonoBehaviour
{
    private bool hasKey1 = false;
    private bool hasKey2 = false;
    private bool hasKey3 = false;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    public GameObject keyUI1;
    public GameObject keyUI2;
    public GameObject keyUI3;

    void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Key1"))
        {
            hasKey1 = true;
            Destroy(other.gameObject);
            keyUI1.SetActive(true);
            
        }

        if (other.CompareTag("Key2"))
        {
            hasKey2 = true;
            Destroy(other.gameObject);
            keyUI2.SetActive(true);

        }


        if (other.CompareTag("Key3"))
        {
            hasKey3 = true;
            Destroy(other.gameObject);
            keyUI3.SetActive(true);

        }


        // Detectar si el jugador toca la puerta invisible y tiene la llave correcta
        if (other.CompareTag("TrigDoor1") && hasKey1)
        {
            Destroy(GameObject.FindWithTag("Door1")); // Destruye la puerta visible
            Destroy(other.gameObject); // Elimina la puerta invisible también
        }
        if (other.CompareTag("TrigDoor2") && hasKey2)
        {
            Destroy(GameObject.FindWithTag("Door2"));
            Destroy(other.gameObject);
        }
        if (other.CompareTag("TrigDoor3") && hasKey3)
        {
            Destroy(GameObject.FindWithTag("Door3"));
            Destroy(other.gameObject);
        }

    }


}
