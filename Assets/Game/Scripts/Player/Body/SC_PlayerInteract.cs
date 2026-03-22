using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SC_PlayerInteract : MonoBehaviour
{
   private PlayerInput playerInput;
   [SerializeField] private LayerMask interactableLayer;
   [SerializeField]private int interactDistance;

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
      Debug.DrawRay(transform.position, transform.forward * interactDistance, Color.red, 1f);
      
      RaycastHit hit;
      if (Physics.Raycast(transform.position, transform.forward, out hit,interactDistance, interactableLayer))
      {
         if (hit.collider.TryGetComponent<SC_InteractableItem>(out var interactableItem))
         {
            interactableItem.Interact();
         }
      }
      //lanzas raycast y compruebas si el gameobjetc tiene la interfaz Interactable
      //Si la tiene le dices que interactue
   }
}
