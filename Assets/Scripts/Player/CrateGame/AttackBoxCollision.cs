using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxCollision : MonoBehaviour
{
    //DEPRECATED
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Box")
        {
            Box box = collider.gameObject.GetComponent<Box>();
            if(box.isActive)
                return;
            
            if(box.type == Box.BoxType.Normal)
            {
                StartCoroutine(box.Move(transform.forward));
                return;
            }

            if(box.type == Box.BoxType.TNT)
            {
                box.Explode(box.explosionRadius);
                StartCoroutine(box.Move(transform.forward));
                return; 
            }

            if(box.type == Box.BoxType.Nitro)
            {
                box.Explode(box.explosionRadius);
            }

            
            
        }
    }
}
