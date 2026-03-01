using UnityEngine;

public class AnimCure : MonoBehaviour
{
    public float rotationSpeed = 50f;  // Velocidad de giro
    public float floatSpeed = 1f;      // Velocidad de flotación
    public float floatHeight = 0.2f;   // Altura de flotación

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0);
    }
}
