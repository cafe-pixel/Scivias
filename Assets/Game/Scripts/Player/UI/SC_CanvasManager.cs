using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SC_CanvasManager : MonoBehaviour
{
    [Header("HUD")]
    [Header("ItemsInteractables")][SerializeField] private GameObject itemTextsUI;
    [SerializeField] private GameObject itemTextsImagesUI;
    [SerializeField] private GameObject itemImagesUI;
    
    [SerializeField] private TextMeshProUGUI itemTextsText;
    [SerializeField] private TextMeshProUGUI itemImagesText;
    [SerializeField] private Image itemImage;
    [SerializeField]  private Image itemImageUIText;
    [Header("UI")]
    [SerializeField] private GameObject messagesUI;
    [SerializeField] private float secondsMessageUI;
    
    [SerializeField] private GameObject contextualButton;
    
    [Header("GameManager")]
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private GameObject gamePauseUI;
    
    private PlayerInput playerInput;
    private bool isInsideTrigger;
    private bool isUIOpen;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        itemTextsUI.SetActive(false);
        itemTextsImagesUI.SetActive(false);
        itemImagesUI.SetActive(false);
        messagesUI.SetActive(false);
        gameOverUI.SetActive(false);
        gamePauseUI.SetActive(false);
        playerInput.actions["ExitUI"].Disable();//Evita que el botón de salir funcione al inicio
        contextualButton.SetActive(false);
        isUIOpen = false;
        isInsideTrigger = false;
    }

    private void OnEnable()
    {
        SC_InteractableItem.OnItemInteract += OnItemInteract;
        SC_TriggerEnemyBegginMove.OnTriggerEnemy += OnTriggerEnemy;
        playerInput.actions["ExitUI"].started += OnExitUI; //press any key para salir de la ui
        SC_InteractableItem.OnTriggerEnterContextualButton += OnTriggerEnterContextualButton;
        SC_InteractableItem.OnTriggerExitContextualButton += OnTriggerExitContextualButton;
    }

    private void OnItemInteract(ItemInfoData data)
    {
        CloseAllUI();
        //realizar switch entre los tipos de data
        switch (data.uiType)
        {
            case UIType.Image:
                itemImagesUI.SetActive(true);
                itemImage.sprite = data.image;
                
                break;
            case UIType.Text:
                itemTextsUI.SetActive(true);
                itemTextsText.text = data.text;
                break;
            case UIType.TextWithImage:
                itemTextsImagesUI.SetActive(true);
                itemImageUIText.sprite = data.image;
                itemImagesText.text = data.text;
                break;
        }
        playerInput.actions["ExitUI"].Enable();
        contextualButton.SetActive(false);
        isUIOpen = true;
    }

    private void OnTriggerExitContextualButton()
    {
        isInsideTrigger = false;
        contextualButton.SetActive(false);
    }

    private void OnTriggerEnterContextualButton()
    {
        isInsideTrigger = true;
        contextualButton.SetActive(true);
    }

    private void OnTriggerEnemy()
    {
        StartCoroutine(EnemyBeginToMove());
    }

    private IEnumerator EnemyBeginToMove()
    {
        messagesUI.SetActive(true);
        yield return new WaitForSeconds(secondsMessageUI);
        messagesUI.SetActive(false);
    }

    

    private void OnExitUI(InputAction.CallbackContext obj)
    {
        CloseAllUI();
        playerInput.actions["ExitUI"].Disable(); //Desactiva el botón
        //no se si el contextual button deberia estar abierto
        isUIOpen = false;
        if (isInsideTrigger)
        {
            contextualButton.SetActive(true);
        }
           
        else
        {
            contextualButton.SetActive(false);
        }
    }

    private void OpenUI(GameObject ui)
    {
        CloseAllUI();
        ui.SetActive(true);
        playerInput.actions["ExitUI"].Enable();
        isUIOpen = true;
        contextualButton.SetActive(false);
    }

    private void OnDisable()
    { 
        
        SC_TriggerEnemyBegginMove.OnTriggerEnemy -= OnTriggerEnemy;
        playerInput.actions["ExitUI"].started -= OnExitUI;
        SC_InteractableItem.OnItemInteract -= OnItemInteract;
        SC_InteractableItem.OnTriggerEnterContextualButton -= OnTriggerEnterContextualButton;
        SC_InteractableItem.OnTriggerExitContextualButton -= OnTriggerExitContextualButton;
    
    }

    
    private void CloseAllUI()
    {
        itemTextsUI.SetActive(false);
        itemTextsImagesUI.SetActive(false);
        itemImagesUI.SetActive(false);
        messagesUI.SetActive(false);
    }
}

