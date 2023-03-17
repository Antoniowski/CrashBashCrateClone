using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
       /*if(collision.gameObject.tag == "Box")
        {
            Box box = collision.gameObject.GetComponent<Box>();
            if(box.type == Box.BoxType.Nitro)
            {
                box.Explode(box.explosionRadius);
            }
        }*/
    }
}
