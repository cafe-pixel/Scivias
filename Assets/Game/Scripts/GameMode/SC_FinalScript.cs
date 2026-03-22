using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<Image> imagesList = new List<Image>();
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip passAudio;
    
      
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
    
    private AudioSource audioSource;
    
    [Header("Typewriter Settings")]
    [Tooltip("20")][SerializeField] private float firstCharacterPerSecond; //cuantas letras x seg van a aparecer
    private float characterPerSecond;
    [Tooltip("0.5")][SerializeField] private float firstInterpunctuationDelay; //delay en signos de puntuacion
    private float interpunctuationDelay;
    
    
    
    
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
        audioSource = GetComponent<AudioSource>();
        cover.SetActive(false);
        interpunctuationDelay = firstInterpunctuationDelay;
        characterPerSecond = firstCharacterPerSecond;
        simpleDelay = new WaitForSeconds(1 / characterPerSecond);
        inpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
        
        textboxFUllEventDelay = new WaitForSeconds(sendDoneDelay);


        foreach (Image image in images)
        {
            imagesList.Add(image);
        }
        audio.volume = Single.MaxValue;
        audio.playOnAwake = false;
        
    }

    private void Start()
    {
        dialogueNumber = 0;
        StartCoroutine(HideImages());
        
        
        audioSource.Play();
        
    }

    private IEnumerator HideImages()
    {
        foreach (var image in imagesList)
        {
            image.gameObject.SetActive(false);
            audio.PlayOneShot(passAudio);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.8f);
        if (dialogueText.Length> dialogueNumber) StartTyping(dialogueText);
        
    }

    private void OnEnable()
    {
        CompleteTextRevealed += WaitAndMainMenu;
      
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
        if (1 <= imageColor.a)
        {
            Debug.Log("Quiero mandarlo a la escena 1");
            SceneManager.LoadScene("MainMenu");

        }
            
    }


    private void OnDisable()
    {
        CompleteTextRevealed -= WaitAndMainMenu;
       
    }

    
    private void StartTyping(string newText)
    {
        if (typeWriterCoroutine != null)
            StopCoroutine(typeWriterCoroutine);

        

        textBox.text = newText;
        textBox.ForceMeshUpdate();
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typeWriterCoroutine = StartCoroutine(TypeWriter());
    }
    
    

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => textBox.maxVisibleCharacters == textBox.textInfo.characterCount - 1);//espera hasta que el numero de char visible sea el numero de contador del text info -1 
    }

    

    private void WaitAndMainMenu()
    {
        StartCoroutine(WaitAndMainMenuCoroutine());
    }

    private IEnumerator WaitAndMainMenuCoroutine()
    {
        yield return new WaitForSeconds(5f);
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
            
            
            if (isPunctuation)
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


