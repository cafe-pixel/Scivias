using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SC_PlayerInteract : MonoBehaviour
{
   private PlayerInput playerInput;

   private void Awake()
   {
      playerInput = GetComponent<PlayerInput>();
   }

   private void OnEnable()
   {
      playerInput.actions["Interact"].started += Interact;
   }

   private void Interact(InputAction.CallbackContext obj)
   {
      //lanzas raycast y compruebas si el gameobjetc tiene la interfaz Interactable
      //Si la tiene le dices que interactue
   }
}
