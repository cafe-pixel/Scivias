using UnityEngine;

public abstract class SC_InteractableItem : MonoBehaviour
{
    public static System.Action OnTriggerEnterContextualButton;
    public static System.Action OnTriggerExitContextualButton;
    public static System.Action<ItemInfoData> OnItemInteract;
    
    [SerializeField] protected ItemInfoData itemInfoData;

    public virtual void Interact()
    {
        OnItemInteract?.Invoke(itemInfoData);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnTriggerEnterContextualButton?.Invoke();
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           OnTriggerExitContextualButton?.Invoke();
        }
    }
}
