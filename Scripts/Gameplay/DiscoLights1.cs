using UnityEngine;

public class DiscoLights1 : MonoBehaviour
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
        float redValue = Random.Range(0.5f, 1f); // Rojo fuerte
        float greenValue = Random.Range(0f, 0.3f); // Poco verde
        float blueValue = Random.Range(0f, 0.3f); // Poco azul

        discoLight.color = new Color(redValue, greenValue, blueValue);
        discoLight.intensity = Random.Range(2f, 8f); // Brillo variable
    }
}
