using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopPanel;        // หน้าร้านค้า
    public Button[] itemButtons;        // ปุ่มสำหรับซื้อของแต่ละชิ้น
    public int[] itemPrices;            // ราคาสินค้าแต่ละชิ้น
    public GameObject[] itemPrefabs;    // Prefab ที่จะ Spawn เมื่อซื้อแต่ละชิ้น
    public Transform spawnPoint;        // จุด Spawn ของที่ซื้อ

    private bool isShopOpen = false;

    void Start()
    {
        // ปิดร้านค้าไว้ก่อน
        if (shopPanel != null)
            shopPanel.SetActive(false);

        // ผูก event ให้ปุ่มทุกปุ่มในร้าน
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i; // สำคัญ! ต้อง copy ค่า i เพื่อไม่ให้เกิดปัญหา lambda capture
            itemButtons[i].onClick.AddListener(() => TryPurchase(index));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.CompareTag("ShopTrigger"))
            {
                ToggleShop();
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.CompareTag("ShopTrigger"))
            {
                ToggleShop();
            }
        }
    }

    void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        if (shopPanel != null)
            shopPanel.SetActive(isShopOpen);
    }

    void TryPurchase(int index)
    {
        if (index < 0 || index >= itemPrices.Length || index >= itemPrefabs.Length)
        {
            Debug.LogWarning("❌ Index ไม่ถูกต้อง");
            return;
        }

        int price = itemPrices[index];

        if (MoneyManager.Instance.SpendMoney(price))
        {
            Debug.Log($"✅ ซื้อของชิ้นที่ {index + 1} ราคา {price} สำเร็จ!");

            if (itemPrefabs[index] != null && spawnPoint != null)
            {
                Instantiate(itemPrefabs[index], spawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("❌ เงินไม่พอ!");
        }
    }
}
