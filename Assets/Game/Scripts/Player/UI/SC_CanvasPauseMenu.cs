using UnityEngine;
using UnityEngine.InputSystem;

public class SC_CanvasPauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    
    private FirstPersonController firstPersonController;
    public GameObject gamePauseCanvas;
    public GameObject HUD;
    
    private PlayerInput playerInput;
    

    private void Awake()
    {
        gamePauseCanvas.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        firstPersonController = GetComponent<FirstPersonController>();
    }

    

    private void OnEnable()
    {
        playerInput.actions["PauseGame"].started += IsGamePaused;
    }
    
    private void OnDisable()
    {
        playerInput.actions["PauseGame"].started -= IsGamePaused;
    }

    private void IsGamePaused(InputAction.CallbackContext obj)
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

    public void Resume()
    {
        Cursor.visible = true;
        gamePauseCanvas.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        HUD.SetActive(true);
        firstPersonController.enabled = true;
    }

    private void Pause()
    {
        Cursor.visible = false;
        gamePauseCanvas.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
        HUD.SetActive(false);
        firstPersonController.enabled = false;
        
    }
}
