using System;
using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_FinalScript : MonoBehaviour
{
    //Quita rápidamente las imágenes y comienza a escribir
    //Va escribiendo el texto
    //Cuando termina espera 3 segundos
    //Te manda a la pantalla de inicio
    [Header("Images")]
    [SerializeField] private Image[] images;
    
      
    [SerializeField]private TMP_Text textBox;
    [Header("Text String")] [TextArea(3, 10)][SerializeField]
    private string dialogueText;

    [SerializeField] private GameObject dialogue;

    private int dialogueNumber;
    
    //Basic Functionality
    private int currentVisibleCharacterIndex;
    private Coroutine typeWriterCoroutine;

    private WaitForSeconds simpleDelay;
    private WaitForSeconds inpunctuationDelay;
    
    
    [Header("Typewriter Settings")]
    [Tooltip("20")][SerializeField] private float firstCharacterPerSecond; //cuantas letras x seg van a aparecer
    private float characterPerSecond;
    [Tooltip("0.5")][SerializeField] private float firstInterpunctuationDelay; //delay en signos de puntuacion
    private float interpunctuationDelay;
    
    
    //Skipping functionality
    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds skipDelay;

    [Header("Skip Options")] [SerializeField]
    private bool quickSpeed;//le mete más velocidad al texto

    [Tooltip("5")][SerializeField] [Min(1)] private int skipSpeedUp; //le acelera la velocidad
    
    
    //Event functionality
    //eventos que ocurren cuando termina de escribir o cosas asi

    private WaitForSeconds textboxFUllEventDelay;
    [Tooltip("0.25")] [SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay;//envia terminar la caja con delay
    private bool readyForNewText;
    [SerializeField]private GameObject cover;
    private bool canUpdateImageColor;

    public static Action CompleteTextRevealed;
    public static Action<char> CharacterRevealer;
    
    //private PlayerInput playerInput;
    

    private void Awake()
    {
        
        cover.SetActive(false);
        interpunctuationDelay = firstInterpunctuationDelay;
        characterPerSecond = firstCharacterPerSecond;
        simpleDelay = new WaitForSeconds(1 / characterPerSecond);
        inpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        skipDelay = new WaitForSeconds(1 / (characterPerSecond * skipSpeedUp)); //te muestra todo el texto rápido
        textboxFUllEventDelay = new WaitForSeconds(sendDoneDelay);

        readyForNewText = true;
    }

    private void Start()
    {
        dialogueNumber = 0;
        if (dialogueText.Length> dialogueNumber) StartTyping(dialogueText);
        
    }
    private void OnEnable()
    {
        CompleteTextRevealed += WaitAndMainMenu;
      // playerInput = GetComponent<PlayerInput>();
      // playerInput.SwitchCurrentActionMap("UIFinal");
      // playerInput.actions["MakeGoFaster"].started += MakeGoFaster;
      // playerInput.actions["MakeGoFaster"].canceled += MakeGoNormalSpeed;
    }

    private void MakeGoFaster(InputAction.CallbackContext obj)
    {
        if (textBox.maxVisibleCharacters <
                textBox.textInfo
                    .characterCount)
            {
                quickSpeed = true;
            }
        else
        {
            WaitAndMainMenu();
        }
        
    }

    private void Update()
    {
        CanUpdateImageColor();
    }

    private void CanUpdateImageColor()
    {
        if (!canUpdateImageColor) return;
        Image image = cover.GetComponent<Image>();
        Color imageColor = image.color;//color de la imagen
        imageColor.a += Time.deltaTime;
        image.color = imageColor;//se asigna el color
        Debug.Log(imageColor.a);
        if (imageColor.a >= 1) 
            SceneManager.LoadScene("MainMenu");
        
    }


    private void OnDisable()
    {
        CompleteTextRevealed -= WaitAndMainMenu;
       //playerInput = GetComponent<PlayerInput>();
       //playerInput.SwitchCurrentActionMap("UIFinal");
       //playerInput.actions["MakeGoFaster"].started -= MakeGoFaster;
       //playerInput.actions["MakeGoFaster"].canceled -= MakeGoNormalSpeed;
    }

    private void MakeGoNormalSpeed(InputAction.CallbackContext obj)
    {
        if (textBox.maxVisibleCharacters <
            textBox.textInfo
                .characterCount)
        {
            quickSpeed = false;
        }
        else
        {
            WaitAndMainMenu();
        }
    }

    private void StartTyping(string newText)
    {
        if (typeWriterCoroutine != null)
            StopCoroutine(typeWriterCoroutine);

        readyForNewText = false;
        CurrentlySkipping = false;

        textBox.text = newText;
        textBox.ForceMeshUpdate();
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typeWriterCoroutine = StartCoroutine(TypeWriter());
    }
    
    

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => textBox.maxVisibleCharacters == textBox.textInfo.characterCount - 1);//espera hasta que el numero de char visible sea el numero de contador del text info -1 
        CurrentlySkipping = false;//se termina de skippear rápido
    }

    

    private void WaitAndMainMenu()
    {
        StartCoroutine(WaitAndMainMenuCoroutine());
    }

    private IEnumerator WaitAndMainMenuCoroutine()
    {
        yield return new WaitForSeconds(3f);
        //Quita el texto y las imágenes
        
        cover.SetActive(true);
        canUpdateImageColor = true;
        
       
    }

    private IEnumerator TypeWriter()
    {
        
        textBox.ForceMeshUpdate();
        TMP_TextInfo textInfo = textBox.textInfo;//recogemos la info del la textBox

        while (currentVisibleCharacterIndex < textInfo.characterCount) //mas uno pq siempre te cuenta el nnumero 0
        {
            
            
            int lastCharacterIndex = textInfo.characterCount - 1;//el ultimo caracter es el ultimo caracter

            if (currentVisibleCharacterIndex == lastCharacterIndex)
            {
                textBox.maxVisibleCharacters++;
                yield return textboxFUllEventDelay;
                CompleteTextRevealed?.Invoke();
                readyForNewText = true;
                yield break;
            }
            
        
            
        
            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character; //le damos al caracter la informacion del caracter que este visible en ese momento
            textBox.maxVisibleCharacters++;//vamos aumentando los caractetres en pantalla
            CharacterRevealer?.Invoke(character);

            

            float delay;

            bool isPunctuation = character == '?' || character == '¿' || character == '¡' ||
                                 character == '!' || character == ',' || character == ';' ||
                                 character == '.' || character == ':' || character == '-';//si es un signo d puntuacion hacemos q lo espere sino no
            
            if (CurrentlySkipping)
            {
                delay = 0f; // si estamos skipeando, no hay delay
            }
            else if (quickSpeed)
            {
                delay = 1f / (firstCharacterPerSecond * skipSpeedUp);
            }
            else if (isPunctuation)
            {
                delay = firstInterpunctuationDelay;
            }
            else
            {
                delay = 1f / firstCharacterPerSecond;
            }

            yield return new WaitForSeconds(delay);
            currentVisibleCharacterIndex++;
        }

        yield return textboxFUllEventDelay;
        CompleteTextRevealed?.Invoke();
        readyForNewText = true;
    }
}


