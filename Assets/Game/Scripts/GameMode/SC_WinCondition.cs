using UnityEngine;

public class SC_WinCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Has ganado
            //Escena de creditos
            //Main menu
        }
    }
}
