using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public GameObject muzzleFlash;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Transform firePoint;
    public Animator _animation;
    public TextMeshProUGUI ammoText;

    public int maxAmmo = 10;
    private int currentAmmo;

    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;



    public float fireForce;
    public float fireRate;
    private bool canFire = true;
    public float damage = 10f;


    public CinemachineVirtualCamera VirtualCamera;
    private Cinemachine.CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public float ShakeDuration = 0.3f;
    public float ShakeAmplitude = 1.2f;
    public float ShakeFrequency = 2.0f;
    private float ShakeElapsedTime = 0f;

    void Start()
    {
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    public void Fire()
    {
        if (!canFire || transform.parent == null || currentAmmo <= 0) return;

        StartCoroutine(FireCooldown());
        currentAmmo--;
        UpdateAmmoUI();

        bool isFlipped = transform.lossyScale.x < 0;
        Vector3 spawnPosition = firePoint.position;

        int bulletCount = gameObject.CompareTag("Shotgun") ? 6 : 1;

        for (int i = 0; i < bulletCount; i++)
        {
            float spreadAngle = gameObject.CompareTag("Shotgun") ? Random.Range(-25f, 25f) : 0f;

            Vector3 shootDirection = Quaternion.AngleAxis(spreadAngle, Vector3.forward) * (isFlipped ? -firePoint.right : firePoint.right);

            GameObject projectile = Instantiate(bullet, spawnPosition, Quaternion.LookRotation(Vector3.forward, shootDirection));

            Bullet bulletScript = projectile.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = damage;
            }

            projectile.GetComponent<Rigidbody2D>().AddForce(shootDirection * fireForce, ForceMode2D.Impulse);
        }

        _animation.SetBool("isFire", true);
        _animation.speed = 0.5f / fireRate;

        GameObject muzzle = Instantiate(muzzleFlash, spawnPosition, firePoint.rotation);
        muzzle.transform.position = firePoint.position;
        muzzle.transform.rotation = firePoint.rotation;
        Destroy(muzzle, 0.1f);

        if (virtualCameraNoise != null)
        {
            ShakeElapsedTime = ShakeDuration;
        }

        audioSource.PlayOneShot(audioClip);
    }



    private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    void Update()
    {

        if (ShakeElapsedTime > 0)
        {
            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

            ShakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            if (virtualCameraNoise != null)
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
            }
        }

        if (transform.parent == null)
        {
            ShakeElapsedTime = 0;

            if (virtualCameraNoise != null)
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = 0f;
            }

            enabled = false;
            return;
        }

        if (canFire)
        {
            _animation.SetBool("isFire", false);
            _animation.speed = 1;
        }

    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }

    }




    public void RefillAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        UpdateAmmoUI();
    }

    public void SetAmmoUI(TextMeshProUGUI ammoUI)
    {
        ammoText = ammoUI;
        UpdateAmmoUI(); // อัปเดตทันทีตอนตั้งค่า
    }

}