using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // 
    public Transform bulletSpawnPoint; // merminin çıkış noktası 
    public Camera playerCamera; // oyuncu kamerası
    public RectTransform aimRectTransform;
    public float bulletSpeed = 50f;
    public float fireRate = 0.2f; // ateş etme hızı
    public float bulletLifetime = 5f; // mermi silinme
    private float nextFireTime = 0f; // bir sonraki ateş zamanı
    public ParticleSystem MuzzleFlash;


    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            MuzzleFlash.Play();
            Shoot();
            nextFireTime = Time.time + fireRate; // bir sonraki ateş zamanını güncelle
        }
    }

    void Shoot()
    {
        Vector2 aimScreenPosition = aimRectTransform.position;

        Ray ray = playerCamera.ScreenPointToRay(aimScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPoint = hit.point;
            
            Quaternion bulletRotation = Quaternion.LookRotation(targetPoint - bulletSpawnPoint.position);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;

            rb.velocity = direction * bulletSpeed;
            
            Destroy(bullet, bulletLifetime);
        }
    }
}
