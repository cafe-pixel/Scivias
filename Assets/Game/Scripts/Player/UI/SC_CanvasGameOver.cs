using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_CanvasGameOver : MonoBehaviour
{
    public static System.Action OnRestart;
    
    [SerializeField] private GameObject GameOverUI;

    private void Awake()
    {
        GameOverUI.SetActive(false);
    }

    public void OnClickRestartButton()
    {
        OnRestart?.Invoke();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void OnClickBackMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        SC_PlayerHealth.On0Life += ShowGameOverCanvas;
    }

    private void ShowGameOverCanvas()
    {
        GameOverUI.SetActive(true);
    }
}
