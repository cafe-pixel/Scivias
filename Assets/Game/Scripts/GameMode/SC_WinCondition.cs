using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_WinCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Win");
        }
    }
}
