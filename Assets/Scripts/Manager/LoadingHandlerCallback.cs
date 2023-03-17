using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHandlerCallback : MonoBehaviour
{
    private bool firstUpdate = true;

    
    void Update()
    {
        //Per far azionare la callback solo una volta dopo il primo frama di Update
        if(firstUpdate)
        {
            LoadingHandler.LoadingCallback();
            firstUpdate = false;
        }
    }
}
