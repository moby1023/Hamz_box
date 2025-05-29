using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{

    public GameObject mapSelectionUI;

    public Button map1;
    public Button map2;
    public Button map3;
    
    public Button close;

    void Start()
    {
        close.onClick.AddListener(Close);

        map1.onClick.AddListener(() => LoadMapScene("Hospital")); 
        map2.onClick.AddListener(() => LoadMapScene("")); 
        map3.onClick.AddListener(() => LoadMapScene(""));
    }

    private void Update()
    {
        
    }

    public void LoadMapScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName); // สำหรับ Debug ดูใน Console
        SceneManager.LoadScene(sceneName);
    }

    public void Close()
    {
        mapSelectionUI.SetActive(false);
    }


}
