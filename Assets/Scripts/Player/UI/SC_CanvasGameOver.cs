using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_CanvasGameOver : MonoBehaviour
{
    public static System.Action OnRestart;
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
}
