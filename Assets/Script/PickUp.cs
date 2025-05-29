using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private Transform ArmPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask weaponLayer;
    [SerializeField] private GameObject weaponUI;

    private PlayerController playerController;
    private GameObject arm;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, weaponLayer);

        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && arm == null)
            {
                PickUpWeapon(hitInfo.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && arm != null)
        {
            DropWeapon();
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.green);
    }

    private void PickUpWeapon(GameObject newWeapon)
    {
        Weapon newWeaponScript = newWeapon.GetComponent<Weapon>();

        if (arm != null)
        {
            Weapon currentWeaponScript = arm.GetComponent<Weapon>();

            // ✅ ถ้าเป็นปืนชนิดเดียวกัน
            if (currentWeaponScript != null && newWeaponScript != null &&
                currentWeaponScript.tag == newWeaponScript.tag)
            {
                currentWeaponScript.RefillAmmo(50); // เติมกระสุน (จำนวนแล้วแต่คุณ)
                Destroy(newWeapon); // ลบปืนที่อยู่บนพื้น
                return; // ไม่ต้องทำขั้นตอนการถือปืนซ้ำ
            }
        }

        // ✅ ถ้ายังไม่มีปืนในมือ → ถือปืนใหม่ตามปกติ
        arm = newWeapon;
        arm.GetComponent<Rigidbody2D>().isKinematic = true;
        arm.transform.position = ArmPoint.position;
        arm.transform.rotation = ArmPoint.rotation;
        arm.transform.SetParent(ArmPoint);

        arm.transform.localScale = new Vector3(transform.localScale.x < 0 ? -1f : 1f, 1f, 1f);

        SpriteRenderer weaponRenderer = arm.GetComponent<SpriteRenderer>();
        if (weaponRenderer != null)
        {
            weaponRenderer.sortingLayerName = "WeaponUp";
            weaponRenderer.sortingOrder = 5;
        }

        if (newWeaponScript != null)
        {
            newWeaponScript.enabled = true;
            newWeaponScript.GetComponent<PolygonCollider2D>().enabled = false;
            newWeaponScript.GetComponent<Animator>().enabled = true;

            if (playerController != null)
            {
                playerController.SetWeapon(newWeaponScript);
            }

            newWeaponScript.SendMessage("UpdateAmmoUI", SendMessageOptions.DontRequireReceiver);

            if (weaponUI != null)
                weaponUI.SetActive(true);
        }
    }



    public void DropWeapon()
    {
        arm.GetComponent<Rigidbody2D>().isKinematic = false;
        arm.transform.SetParent(null);

        SpriteRenderer weaponRenderer = arm.GetComponent<SpriteRenderer>();
        if (weaponRenderer != null)
        {
            weaponRenderer.sortingLayerName = "Weapons";
            weaponRenderer.sortingOrder = 2;
        }

        if (arm.GetComponent<Weapon>() != null)
        {
            arm.GetComponent<Weapon>().enabled = false;
            arm.GetComponent<PolygonCollider2D>().enabled = true;
            arm.GetComponent<Animator>().enabled = false;
        }

        if (weaponUI != null)
            weaponUI.SetActive(false); // ✅ ซ่อน UI

        arm = null;
    }


    public void TryPickUp()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, weaponLayer);

        if (hitInfo.collider != null && arm == null)
        {
            PickUpWeapon(hitInfo.collider.gameObject);
        }
    }




}