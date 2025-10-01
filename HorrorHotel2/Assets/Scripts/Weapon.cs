using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Weapon Stats")]
    public int magSize = 3;               
    public float reloadTime = 2f;         
    public float shootingDelay = 0.2f;    
    public float bulletVelocity = 30f;    
    public float bulletPrefabLifeTime = 3f;

    [Header("Spread")]
    public float spreadIntensity = 0.1f;  

    [Header("States")]
    public bool isShooting;
    public bool readyToShoot;
    public bool isReloading;

    private bool allowReset = true;
    public int bulletsLeft;              
    private int burstBulletsLeft;

    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode currentShootingMode;

    void Awake()
    {
        readyToShoot = true;
        bulletsLeft = magSize;
        burstBulletsLeft = magSize;
    }

    void Update()
    {
        // --- Input ---
        if (currentShootingMode == ShootingMode.Auto)
            isShooting = Input.GetKey(KeyCode.Mouse0);
        else
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reload input
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !isReloading)
            StartCoroutine(Reload());

        

        // Shooting
        if (readyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft == 0)
                burstBulletsLeft = magSize; // reset burst

            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        // Shoot one bullet
        bulletsLeft--;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootingDirection * bulletVelocity;

        Destroy(bullet, bulletPrefabLifeTime);

        // Reset shot delay
        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        // Handle Burst
        if (currentShootingMode == ShootingMode.Burst)
        {
            burstBulletsLeft--;
            if (burstBulletsLeft > 0 && bulletsLeft > 0)
                Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100);

        Vector3 direction = targetPoint - bulletSpawn.position;

        float X = Random.Range(-spreadIntensity, spreadIntensity);
        float Y = Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(X, Y, 0);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        readyToShoot = false;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        bulletsLeft = magSize;
        burstBulletsLeft = magSize;

        isReloading = false;
        readyToShoot = true;

        Debug.Log("Reload Complete!");
    }
}
