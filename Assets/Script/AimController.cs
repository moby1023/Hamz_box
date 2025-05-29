using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 originalScale;
    private PlayerInput playerInput;
    private InputAction lookAction;

    private Vector2 lookInput;

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        originalScale = transform.localScale;

        playerInput = playerTransform.GetComponent<PlayerController>().playerControls;
        lookAction = playerInput.Player.Look;
        lookAction.Enable();
    }

    void Update()
    {
        lookInput = lookAction.ReadValue<Vector2>();

        if (lookInput.sqrMagnitude > 0.1f)
        {
            Vector3 direction = new Vector3(lookInput.x, lookInput.y, 0f);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.position = playerTransform.position;

            // Flip Arm (บน-ล่าง)
            if (angle > 90 || angle < -90)
            {
                transform.localScale = new Vector3(originalScale.x, -originalScale.y, originalScale.z);
            }
            else
            {
                transform.localScale = originalScale;
            }

            // ✅ Flip Player (ซ้าย-ขวา)
            if (lookInput.x > 0)
            {
                playerTransform.rotation = Quaternion.Euler(0, 0, 0);  // หันขวา
            }
            else if (lookInput.x < 0)
            {
                playerTransform.rotation = Quaternion.Euler(0, 180, 0);  // หันซ้าย
            }
        }
    }

}
