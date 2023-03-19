using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public int numberOfHumanPlayer;

    public PlayerInfo player01;
    public PlayerInfo player02;
    public PlayerInfo player03;
    public PlayerInfo player04;

    [HideInInspector] public bool startGame = false;
    bool alreadyStarted = false;

    public float timer = 60;


    void Update()
    {
        float delta = Time.deltaTime;
        if(startGame && !alreadyStarted)
        {
            alreadyStarted = true;
            GameStart(delta);
        }
    }

    void GameStart(float delta)
    {
        //Inizia il timer
        StartCoroutine(TimerHandler(delta));
    }

    IEnumerator TimerHandler(float delta)
    {
        while(timer > 0)
        {
            timer -= delta;
            yield return null;
        }
    }

}
