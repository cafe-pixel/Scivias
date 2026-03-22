using System;
using UnityEngine;

public class SC_TriggerEnemyBegginMove : MonoBehaviour
{
    public static System.Action OnTriggerEnemy;
    private int playerStepIntTimes;

    private void Start()
    {
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& playerStepIntTimes <1)
        {
            OnTriggerEnemy?.Invoke();
            playerStepIntTimes++;
        }
    }
}
