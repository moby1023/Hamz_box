using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopUIManager : MonoBehaviour
{
    public GameObject ShopUI;
    public Button CloseButton;

    void Start()
    {
        CloseButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        ShopUI.SetActive(false);
    }
}