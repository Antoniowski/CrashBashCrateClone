using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStateManager : MonoBehaviour
{
    [SerializeField] State currentState;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToNextState(nextState);
        }

    }

    public void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }    
}
