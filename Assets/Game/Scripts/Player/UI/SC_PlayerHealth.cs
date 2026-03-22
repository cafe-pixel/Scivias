using System;
using UnityEngine;

public class SC_PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initialHealth;
    private int currentHealth;
    
    public static System.Action On0Life;

    private void Awake()
    {
        currentHealth = initialHealth;
    }

    private void OnEnable()
    {
        SC_CanvasGameOver.OnRestart += RestartLife;
    }

    private void RestartLife()
    {
        currentHealth = initialHealth;
    }

    private void OnDisable()
    {
        SC_CanvasGameOver.OnRestart -= RestartLife;
    }

    public void ReciveDamage()
    {
        currentHealth--;
        if (currentHealth <= 0) On0Life?.Invoke();
    }
}
