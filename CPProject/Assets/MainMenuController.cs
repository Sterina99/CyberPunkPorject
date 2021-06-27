using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
   [SerializeField] GameObject menuPanel;
   [SerializeField] GameObject creditsPanel;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCredits()
    {
        creditsPanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);

    }
  public void GoToMenu()
    {
        menuPanel.gameObject.SetActive(true);
        creditsPanel.gameObject.SetActive(false);

    }
}
