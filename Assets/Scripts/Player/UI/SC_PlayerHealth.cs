using System;
using UnityEngine;

public class SC_PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initialHealth;
    private int currentHealth;

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
}
