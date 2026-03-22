using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SC_PlayerInteract : MonoBehaviour
{
   private PlayerInput playerInput;
   [SerializeField] private LayerMask interactableLayer;
   [SerializeField]private int interactDistance;
   
   public static System.Action OnTryToInteract;

   private void Awake()
   {
      playerInput = GetComponent<PlayerInput>();
   }

   private void OnEnable()
   {
      playerInput.actions["Interact"].started += Interact;
   }
   private void OnDisable()
   {
      playerInput.actions["Interact"].started -= Interact;
   }

   private void Interact(InputAction.CallbackContext obj)
   {
      Debug.Log("Interact");
      
      OnTryToInteract?.Invoke();
      
   }
}
