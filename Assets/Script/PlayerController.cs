using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI ammoTextUI;
    [SerializeField] private PickUp pickUpSystem;
    [SerializeField] private UnityEngine.UI.Slider healthBar; // สำหรับแสดงเลือด (ถ้ามี UI)

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector3 originalScale;
    public Weapon weapon;
    public Animator _animation;

    [Header("Health System")]
    public float maxHealth = 100f;
    public float currentHealth;

    public PlayerInput playerControls;

    private InputAction move;
    private InputAction pickUp;
    private InputAction look;
    private InputAction drop;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    Vector2 velocity;

    private void Awake()
    {
        playerControls = new PlayerInput();
    }

    void Start()
    {
        originalScale = transform.localScale;
        currentHealth = maxHealth;

        if (virtualCamera != null)
        {
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        UpdateHealthUI();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        pickUp = playerControls.Player.Pick;
        pickUp.Enable();
        pickUp.performed += ctx => Pick();

        look = playerControls.Player.Look;
        look.Enable();

        drop = playerControls.Player.Drop;
        drop.Enable();
        drop.performed += ctx => Drop();
    }

    private void OnDisable()
    {
        move.Disable();
        pickUp.Disable();
        look.Disable();
        drop.Disable();
    }

    void Update()
    {
        Vector2 aim = look.ReadValue<Vector2>();
        bool isAiming = aim.sqrMagnitude > 0.1f;

        velocity = move.ReadValue<Vector2>();

        _animation.SetBool("isWalking", velocity != Vector2.zero);

        if (weapon != null && Input.GetMouseButton(0) && isAiming)
        {
            weapon.Fire();
        }

        UpdateHealthUI();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;

        // ให้ weapon รู้จัก ammoTextUI
        if (weapon != null && ammoTextUI != null)
        {
            weapon.SetAmmoUI(ammoTextUI);
        }
    }

    private void Pick()
    {
        pickUpSystem.TryPickUp();
    }

    private void Drop()
    {
        pickUpSystem.DropWeapon();
    }

    // ===== Health System =====

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthUI();
    }

    private void Die()
    {
        Debug.Log("Player Died");

        // หยุดการเคลื่อนไหวและอินพุต
        move.Disable();
        pickUp.Disable();
        look.Disable();
        drop.Disable();

        // แสดง Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // หยุดยิงหรือทำสิ่งอื่นๆ
        _animation.SetBool("isWalking", false);
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player healed {amount} HP. Current health: {currentHealth}");
        UpdateHealthUI();
    }


}


