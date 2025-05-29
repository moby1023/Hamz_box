using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    public GameObject tutorialUI;
    public Button CloseButton;

    void Start()
    {
        CloseButton.onClick.AddListener(Close);
    }

    public void Close()
    {
            tutorialUI.SetActive(false); 
    }
}
