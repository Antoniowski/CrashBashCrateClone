using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
    [SerializeField] float secondsToWait = 2.5f;
    void Awake()
    {
        StartCoroutine(CleanScene(secondsToWait));
    }
    
    IEnumerator CleanScene(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        Destroy(gameObject);
    }

    }
