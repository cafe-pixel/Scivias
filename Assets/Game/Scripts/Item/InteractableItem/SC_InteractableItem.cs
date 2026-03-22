using System;
using UnityEngine;

public class SC_InteractableItem : MonoBehaviour
{
    public static System.Action OnTriggerEnterContextualButton;
    public static System.Action OnTriggerExitContextualButton;
    public static System.Action<ItemInfoData> OnItemInteract;

    private bool canInteract;
    
    [SerializeField] protected ItemInfoData itemInfoData;

    private void Interact()
    {
        if(canInteract) OnItemInteract?.Invoke(itemInfoData);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnTriggerEnterContextualButton?.Invoke();
            canInteract = true;
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           OnTriggerExitContextualButton?.Invoke();
           canInteract = false;
        }
    }

    private void OnEnable()
    {
        SC_PlayerInteract.OnTryToInteract  += Interact;
    }
}
