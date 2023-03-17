using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] GameObject loadingText;
    Quaternion rotateTo;
    void Start()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().StopAllMusic();
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayTargetSound("LoadingTheme");
    }

    void LateUpdate()
    {
        LoadingAnimation();
    }

    void LoadingAnimation()
    {
        loadingText.transform.rotation = Quaternion.Euler(10*Mathf.Sin(5*Time.time),10*Mathf.Cos(5*Time.time),8*Mathf.Sin(3*Time.time));
        //loadingText.transform.position = new Vector3(loadingText.transform.position.x, loadingText.transform.position.y + 0.1f*Mathf.Sin(5f*Time.time), loadingText.transform.position.z);
    }       
}
