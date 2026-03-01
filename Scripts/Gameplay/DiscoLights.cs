using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    public Light discoLight; // Arrastra aquí la luz en el Inspector
    public float changeInterval = 0.2f; // Tiempo entre cambios de color
    public float rotationSpeed = 50f;

    void Start()
    {
        InvokeRepeating("ChangeLightColor", 0, changeInterval);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void ChangeLightColor()
    {
        discoLight.color = new Color(Random.value, Random.value, Random.value);
        discoLight.intensity = Random.Range(2f, 8f); // Brillo variable
    }
}
