using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotReachBoxState : State
{

   BotManager botManager;
   BotMovementManager botMovementManager;

   [SerializeField] State grabState;
   [SerializeField] State searchState;
   public bool reachedBox = false;

   void Awake()
   {
      botManager = GetComponentInParent<BotManager>();
      botMovementManager = GetComponentInParent<BotMovementManager>();
   }

   public override State RunCurrentState()
   {

         if(reachedBox)
         {
            //GRAB
            reachedBox = false;
            botManager.isMoving = false;
            Debug.Log("REACH STATE -> GRAB STATE");
            return grabState;
            
         }

         //Si muove verso la scatola;
         if(!botManager.isMoving)
            botManager.isMoving = true;
         
         if(botMovementManager.targetBox.isPicked)
         {
            reachedBox = false;
            botManager.isMoving = false;
            botMovementManager.targetBox = null;
            Debug.Log("REACH STATE -> SEARCH STATE");
            return searchState;
         }

        return this;
   }
}
