using UnityEngine;

public class SC_CanvasPauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    
    public GameObject playerBody;
    public GameObject gamePauseCanvas;
    public GameObject canvas;
    

    private void Awake()
    {
        gamePauseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        gamePauseCanvas.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        canvas.SetActive(true);
        playerBody.SetActive(true);
    }

    private void Pause()
    {
        gamePauseCanvas.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
        canvas.SetActive(false);
        playerBody.SetActive(false);
        
    }
}
