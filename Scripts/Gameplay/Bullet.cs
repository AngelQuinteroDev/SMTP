using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // Comprobar si golpeamos algo con un componente Health
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // Puedes instanciar un efecto de impacto aquí
        // Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));

        // Destruir la bala al impactar
        Destroy(gameObject);
    }
}