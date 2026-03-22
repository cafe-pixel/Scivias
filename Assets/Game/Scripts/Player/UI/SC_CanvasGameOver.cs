using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_CanvasGameOver : MonoBehaviour
{
    public static System.Action OnRestart;
    
    private FirstPersonController firstPersonController;
    
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject PauseUI;

    private void Awake()
    {
        GameOverUI.SetActive(false);
        firstPersonController = GetComponent<FirstPersonController>();
    }

    public void OnClickRestartButton()
    {
        Debug.Log("Restart");
        
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        OnRestart?.Invoke();
        
    }

    public void OnClickBackMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable()
    {
        SC_PlayerHealth.On0Life += ShowGameOverCanvas;
    }
    
    private void OnDisable()
    {
        SC_PlayerHealth.On0Life -= ShowGameOverCanvas;
    }

    private void ShowGameOverCanvas()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverUI.SetActive(true);
        PauseUI.SetActive(false);
        Time.timeScale = 0;
    }
}
