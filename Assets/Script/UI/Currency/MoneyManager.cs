using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("UI")]
    public TextMeshProUGUI moneyText;

    [Header("Starting Money")]
    public int startingMoney = 100;

    private int currentMoney;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentMoney = startingMoney;
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("❌ เงินไม่พอ!");
            return false;
        }
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"$ : {currentMoney}";
        }
    }
}
