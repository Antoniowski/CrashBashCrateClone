using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    private int vertical;
    private int horizontal;

    public void Init(){
        animator = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementValues(float verticalMovement, float horizontalMovement)
    {
        #region Vertical
        float v;

        if(verticalMovement > 0){
            v = 1f;
        }else if(verticalMovement < 0){
            v = -1f;
        }else{
            v = 0;
        }
        #endregion


        #region Horizontal
        float h;
        if(horizontalMovement > 0){
            h = 0.5f;
        }else if (horizontalMovement < 0){
            h = -0.5f;
        }else{
            h = 0;
        }
        #endregion

        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);

    }

    public void PlaySimpleAnimation(string targetAnim)
    {
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayAnimationTarget(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
        
    }

    public void PlayAnimationTargetNO_INTERACTING(string targetAnim)
    {
        animator.applyRootMotion = true;
        animator.SetBool("IsInteracting", false);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayAnimationTarget(Animation targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnim.name, 0.2f);
    }
}
