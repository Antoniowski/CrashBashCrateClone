using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player = null;
    private float fi = 45;
    private float theta = 270;
    public float distance = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
            
        //Set camera on the player using spheric coordinates
        SetAndHoldCameraPosition();
        //Rotate to 45 degrees
        transform.rotation = Quaternion.Euler(new Vector3(fi, 0, 0));
    }




    // Update is called once per frame
    void Update()
    {
        SetAndHoldCameraPosition();
    }

    
    
    
    void SetAndHoldCameraPosition(){
        float xValue = player.transform.position.x + distance*Mathf.Cos(fi*Mathf.PI/180)*Mathf.Cos(theta*Mathf.PI/180);
        float zValue = player.transform.position.z + distance*Mathf.Sin(theta*Mathf.PI/180)*Mathf.Cos(fi*Mathf.PI/180);
        float yValue = player.transform.position.y + distance*Mathf.Sin(fi*Mathf.PI/180);
        transform.position = new Vector3(xValue, yValue, zValue);
    }
}
