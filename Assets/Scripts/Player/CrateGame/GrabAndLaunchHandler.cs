using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrabAndLaunchHandler : MonoBehaviour
{

    PlayerManager playerManager;
    Animator animator;
    AnimationHandler animationHandler;

    [SerializeField] float ThrowPower = 10;
    [SerializeField] BoxCollider grabBox;
    public GameObject pickedBox;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponentInChildren<Animator>();
        animationHandler = GetComponent<AnimationHandler>();

    }
    public void HandleGrab()
    {
        grabBox.enabled = true;
        StartCoroutine(DisableGrabBox());
    }

    IEnumerator DisableGrabBox()
    {
        yield return new WaitForSeconds(0.5f);
        grabBox.enabled = false;
    }

    public void HandleLaunch()
    {   
        playerManager.hasBox = false;
        //Rimuovere dal parent
        pickedBox.transform.parent = null;
        //rendere "attiva" la cassa
        Box box = pickedBox.GetComponent<Box>();
        box.isThrown = true;
        //aggiungere RigidBody
        Rigidbody rd =  pickedBox.AddComponent<Rigidbody>();
        rd.AddForce(transform.forward*ThrowPower, ForceMode.Impulse);
        pickedBox.GetComponentInParent<BoxCollider>().enabled = true;
        animator.CrossFade("Grab Empty",0.2f);
        animationHandler.PlayAnimationTarget("Throw", true);
        pickedBox = null;
        

        //StartCoroutine(ThrowBox());

    }

    /*IEnumerator ThrowBox()
    {
        while
    }*/

}
