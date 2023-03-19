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
    CharacterPrefabManager prefabManager;

    [Header("PULSANTI")]
    [SerializeField] GameObject crashButton;
    [SerializeField] GameObject cocoButton;
    [SerializeField] GameObject tinyButton;
    [SerializeField] GameObject crocoButton;
    [SerializeField] GameObject cortexButton;
    [SerializeField] GameObject brioButton;
    [SerializeField] GameObject kongButton;
    [SerializeField] GameObject rillaButton;

    [Header("POSIZIONI MODELLI")]
    [SerializeField] GameObject P1;
    [SerializeField] GameObject P2;
    [SerializeField] GameObject P3;
    [SerializeField] GameObject P4;

    [SerializeField] AnimationCurve spawnAnimation;

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
        prefabManager = GameObject.Find("CharacterPrefabManager").GetComponent<CharacterPrefabManager>();
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
                matchManager.player01 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character, prefabManager.GetCharacterController(true, character));
                break;
            case 2:
                matchManager.player02 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character,prefabManager.GetCharacterController(true, character));
                break;
            case 3:
                matchManager.player03 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character,prefabManager.GetCharacterController(true, character));
                break;
            case 4:
                matchManager.player04 = new PlayerInfo(selectionIndex, false, PlayerInfo.ControllType.Gamepad, character,prefabManager.GetCharacterController(true, character));
                break;
            default:
                Debug.LogWarning("No player selected");
                break;
        }

        playerSelectedCharacter += 1;

        //Controlla se tutti i player hanno selezionato un personaggio
        if(playerSelectedCharacter != matchManager.numberOfHumanPlayer)
            return;
        
        
        //RANDOM CPU PLAYER
        GenerateCPUPlayer();
        
        //NO! CRASHA IL MONDO
        StartCoroutine(AppearAnimation());

        Debug.Log("P1:" + matchManager.player01.character + " P2:" + matchManager.player02.character + " P3:" + matchManager.player03.character + "P4:" + matchManager.player04.character);

        //StartCoroutine(DelayLoading());
        //LoadingHandler.Load(LoadingHandler.Scene.LoadingScene);
    }

    void GenerateCPUPlayer()
    {
        System.Random rnd = new System.Random();
        List<string> charactersList = new List<string>();
        foreach(PrefabHolder prefabHolder in prefabManager.characterArray)
        {
            charactersList.Add(prefabHolder.name);
        }

        string[] characters = charactersList.ToArray();

        int playerLeft = 4 - matchManager.numberOfHumanPlayer;
        print(playerLeft);
        
        while (playerLeft > 0)
        {
            if(matchManager.player02.ID == 0)
            {
                int randomInt = rnd.Next(0, characters.Length);
                string character = characters[randomInt];
                matchManager.player02 = new PlayerInfo(2, true, PlayerInfo.ControllType.Keyboard, character, prefabManager.GetCharacterController(false, character));
                
            }
            else if(matchManager.player03.ID == 0)
            {
                int randomInt = rnd.Next(0, characters.Length);
                string character = characters[randomInt];
                matchManager.player03 = new PlayerInfo(3, true, PlayerInfo.ControllType.Keyboard, character, prefabManager.GetCharacterController(false, character));    
            }
            else if(matchManager.player04.ID == 0)
            {
                int randomInt = rnd.Next(0, characters.Length);
                string character = characters[randomInt];
                matchManager.player04 = new PlayerInfo(4, true, PlayerInfo.ControllType.Keyboard, character, prefabManager.GetCharacterController(false, character));    
            }
            playerLeft -= 1;
        }
        
    }

    /*void SetUPCPU(System.Random rnd, Character[] characters)
    {
        
        Character randomChar = (Character)characters.GetValue(rnd.Next(characters.Length));
        matchManager.player02 = new PlayerInfo(2, true, PlayerInfo.ControllType.Keyboard, randomChar.ToString());
    }*/

    //SUONI
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

    //ANIMAZIONE
    IEnumerator AppearAnimation()
    {
        int index = 4;
        while(index > 0)
        {
            if(index == 4)
            {
                index-=1;
                GameObject gameObject = Instantiate(Array.Find(prefabManager.characterArray, c => c.name == matchManager.player01.character).model) as GameObject;
                gameObject.transform.rotation = Quaternion.Euler(0,180,0);
                gameObject.transform.localScale = new Vector3(1,0,1);
                gameObject.transform.position = P1.transform.position;
                audioManager.PlayTargetSound("Boing");
                StartCoroutine(SimpleAnimation(gameObject));
                yield return new WaitForSeconds(0.80f);
            }
            if(index == 3)
            {
                index-=1;
                GameObject gameObject = Instantiate(Array.Find(prefabManager.characterArray, c => c.name == matchManager.player02.character).model) as GameObject;
                gameObject.transform.rotation = Quaternion.Euler(0,180,0);
                gameObject.transform.localScale = new Vector3(1,0,1);
                gameObject.transform.position = P2.transform.position;
                audioManager.PlayTargetSound("Boing");
                StartCoroutine(SimpleAnimation(gameObject));
                yield return new WaitForSeconds(0.80f);
            }
            if(index == 2)
            {
                index-=1;
                GameObject gameObject = Instantiate(Array.Find(prefabManager.characterArray, c => c.name == matchManager.player03.character).model) as GameObject;
                gameObject.transform.rotation = Quaternion.Euler(0,180,0);
                gameObject.transform.localScale = new Vector3(1,0,1);
                gameObject.transform.position = P3.transform.position;
                audioManager.PlayTargetSound("Boing");
                StartCoroutine(SimpleAnimation(gameObject));
                yield return new WaitForSeconds(0.80f);             
            }
            if(index == 1)
            {
                index-=1;
                GameObject gameObject = Instantiate(Array.Find(prefabManager.characterArray, c => c.name == matchManager.player04.character).model) as GameObject;
                gameObject.transform.rotation = Quaternion.Euler(0,180,0);
                gameObject.transform.localScale = new Vector3(0,0,0);
                gameObject.transform.position = P4.transform.position;
                audioManager.PlayTargetSound("Boing");
                StartCoroutine(SimpleAnimation(gameObject));
                yield return new WaitForSeconds(0.80f);             
            }

        }

        StartCoroutine(DelayLoading());
    }

    IEnumerator SimpleAnimation(GameObject g)
    {
        float value = 0;
        float time = 0;
        while (time < 1.5f)
        {   
            time += Time.deltaTime;
            value = spawnAnimation.Evaluate(time);
            g.transform.localScale = new Vector3(value ,value, value);
            yield return null;
        }
    }

    IEnumerator DelayLoading()
    {
        yield return new WaitForSeconds(4f);
        LoadingHandler.Load(LoadingHandler.Scene.LoadingScene);
    }
}
