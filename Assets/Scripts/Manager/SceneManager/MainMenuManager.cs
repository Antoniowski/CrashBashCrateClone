using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{

    InputManager inputManager;
    AudioManager audioManager;

    //Buttons
    [SerializeField] GameObject newGameButton;
    [SerializeField] GameObject exitButton;


    // Start is called before the first frame update
    void Start()
    {
        inputManager =  FindObjectOfType<InputManager>();//GameObject.Find("InputManager").GetComponent<InputManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayTargetSound("MainMenuTheme");
        EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager.vertical != 0 || inputManager.horizontal != 0)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(newGameButton);
        }
    }

    public void NewGameButton()
    {
        GameObject matchHandler = new GameObject("MatchHandler");
        MatchManager matchManager = matchHandler.AddComponent<MatchManager>();
        matchManager.numberOfHumanPlayer = 1;
        DontDestroyOnLoad(matchHandler);
        SceneManager.LoadScene(LoadingHandler.Scene.CharacterSelection.ToString());
        //LoadingHandler.Load(LoadingHandler.Scene.CharacterSelection);
    }

    public void PlaySelectButtonSound()
    {
        audioManager.PlayTargetSound("Beep");
    }
    
}
