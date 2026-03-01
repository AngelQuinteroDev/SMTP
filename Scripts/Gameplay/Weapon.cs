using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Configuración de Arma")]
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private bool automatic = false;
    [SerializeField] private int magazineSize = 12;
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Configuración de Efectos")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private Light muzzleFlashLight;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;

    // Variables privadas
    private AudioSource audioSource;
    private float nextFire = 0.0f;
    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        if (fpsCam == null && Camera.main != null)
            fpsCam = Camera.main;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (bulletSpawnPoint == null)
            bulletSpawnPoint = transform;

        currentAmmo = magazineSize;

        // Apagar la luz al inicio
        if (muzzleFlashLight != null)
            muzzleFlashLight.enabled = false;
    }

    void Update()
    {
        if (!isReloading)
        {
            bool shouldFire = automatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

            if (shouldFire && Time.time > nextFire && currentAmmo > 0)
            {
                nextFire = Time.time + fireRate;
                Shoot();
                currentAmmo--;

                if (currentAmmo <= 0)
                    StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < magazineSize)
            StartCoroutine(Reload());
    }

    void Shoot()
    {
        // Activar efectos visuales y sonoros
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = true;
            StartCoroutine(DisableMuzzleLight());
        }

        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);

        // Disparar raycast
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            targetPoint = hit.point;
            Debug.Log("Hit: " + hit.transform.name);

            if (hit.transform.CompareTag(enemyTag))
            {
                Health health = hit.transform.GetComponent<Health>();
                if (health != null)
                    health.TakeDamage(damage);
                else
                    Destroy(hit.transform.gameObject);
            }

            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }

            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * hit.distance, Color.yellow, 1f);
        }
        else
        {
            targetPoint = fpsCam.transform.position + fpsCam.transform.forward * range;
            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * range, Color.red, 1f);
        }

        // Crear bala
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Vector3 bulletDirection = (targetPoint - bulletSpawnPoint.position).normalized;
            float bulletDuration = Vector3.Distance(bulletSpawnPoint.position, targetPoint) / bulletSpeed;
            StartCoroutine(MoveBullet(bullet, bulletSpawnPoint.position, targetPoint, bulletDuration));
        }
    }

    IEnumerator DisableMuzzleLight()
    {
        yield return new WaitForSeconds(0.05f);
        if (muzzleFlashLight != null)
            muzzleFlashLight.enabled = false;
    }

    IEnumerator MoveBullet(GameObject bullet, Vector3 startPoint, Vector3 endPoint, float duration)
    {
        float startTime = Time.time;
        bullet.transform.forward = (endPoint - startPoint).normalized;

        while (Time.time < startTime + duration)
        {
            float timeProgress = (Time.time - startTime) / duration;
            bullet.transform.position = Vector3.Lerp(startPoint, endPoint, timeProgress);
            yield return null;
        }

        bullet.transform.position = endPoint;
        Destroy(bullet);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        Debug.Log("Recargando...");
        yield return new WaitForSeconds(2f);

        currentAmmo = magazineSize;
        isReloading = false;
        Debug.Log("Recarga completada!");
    }
}
