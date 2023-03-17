using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CharacterSelectionHandler : MonoBehaviour
{


    public enum Character
    {
        Crash,
        Coco,
        Tiny,
        Dingo,
        Cortex,
        Brio,
        Kong, 
        Rilla
    }

    InputManager inputManager;
    AudioManager audioManager;
    MatchManager matchManager;

    //Buttons
    [SerializeField] GameObject crashButton;
    [SerializeField] GameObject cocoButton;
    [SerializeField] GameObject tinyButton;
    [SerializeField] GameObject crocoButton;
    [SerializeField] GameObject cortexButton;
    [SerializeField] GameObject brioButton;
    [SerializeField] GameObject kongButton;
    [SerializeField] GameObject rillaButton;

    private int selectionIndex;
    private int playerSelectedCharacter;



    // Start is called before the first frame update
    void Start()
    {
        inputManager =  FindObjectOfType<InputManager>();//GameObject.Find("InputManager").GetComponent<InputManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        matchManager = GameObject.Find("MatchHandler").GetComponent<MatchManager>();
        selectionIndex = 1;
        playerSelectedCharacter = 0;
        EventSystem.current.SetSelectedGameObject(crashButton);
        //EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    // Update is called once per frame
    void Update()
    {
        //Per selezionare il primo bottone
        if(inputManager.vertical != 0 || inputManager.horizontal != 0)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(crashButton);
        }
    }

    public void CharacterSelected(string character)
    {
        switch(selectionIndex)
        {
            case 1:
                matchManager.player01 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character);
                break;
            case 2:
                matchManager.player02 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character);
                break;
            case 3:
                matchManager.player03 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character);
                break;
            case 4:
                matchManager.player04 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character);
                break;
            default:
                Debug.LogWarning("No player selected");
                break;
        }

        //Controlla se tutti i player hanno selezionato un personaggio
        if(playerSelectedCharacter != matchManager.numberOfHumanPlayer)
        {
            playerSelectedCharacter += 1;
            return;
        }
        
        LoadingHandler.Load(LoadingHandler.Scene.LoadingScene);
    }

    void GenerateCPUPlayer()
    {
        System.Random rnd = new System.Random();
        Array characters = Enum.GetValues(typeof(Character));
        int playerLeft = 4 - matchManager.numberOfHumanPlayer;
        
        while (playerLeft > 0)
        {
            if(matchManager.player02 != null)
            {
                Character randomChar = (Character)characters.GetValue(rnd.Next(characters.Length));
                int randomCharIndex = Array.IndexOf(characters, randomChar);
                

                matchManager.player02 = new PlayerInfo(2, true, PlayerInfo.ControllType.Keyboard, randomChar.ToString());
            }
            else if(matchManager.player03 != null)
            {
                
            }
            else if(matchManager.player04 != null)
            {

            }
        }
        
    }

    void SetUPCPU(System.Random rnd, Character[] characters)
    {
        
        Character randomChar = (Character)characters.GetValue(rnd.Next(characters.Length));
        matchManager.player02 = new PlayerInfo(2, true, PlayerInfo.ControllType.Keyboard, randomChar.ToString());
    }

    public void PlaySelectButtonSound(bool isKong)
    {
        if(isKong)
        {
            audioManager.PauseClip("MainMenuTheme");
            audioManager.PlayTargetSound("KongTheme");
            return;
        }

        if(!isKong)
        {
            audioManager.StopClip("KongTheme");
            audioManager.ResumeClip("MainMenuTheme");
        }
        audioManager.PlayTargetSound("Beep");
    }
}
