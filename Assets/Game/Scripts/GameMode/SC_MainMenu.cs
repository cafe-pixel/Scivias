using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{
    private Button button;
    [SerializeField] private float newColorMultiplier;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject jumpScare;
   

    private void Awake()
    {
        panel.SetActive(false);
        jumpScare.SetActive(false);
        button = GetComponent<Button>();
        
    }

    public void OnClickStartGame()
    {
        panel.SetActive(true);
        jumpScare.SetActive(true);
        StartCoroutine(WaitOneSecondePlease());
    }

    private IEnumerator WaitOneSecondePlease()
    {
        
        yield return new WaitForSeconds(0.07f);
        SceneManager.LoadScene("Game");
        yield break;
    }

    public void OnClickQuitGame()
    {
        Application.Quit();
    }

    public void OnPointerStay(EventSystem eventSystem)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.colorMultiplier *= newColorMultiplier;
        button.colors = colorBlock;
    }
    
    public void OnPointerExit(EventSystem eventSystem)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.colorMultiplier = 1;
        button.colors = colorBlock;
    }

    public void OnPlip(EventSystem eventSystem)
    {
        audioSource.Play();
    }
    
    
    
    
    
}

