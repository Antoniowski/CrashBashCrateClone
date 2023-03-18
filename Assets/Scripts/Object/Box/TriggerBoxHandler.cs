using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxHandler : MonoBehaviour
{
    Box box;

    void Awake()
    {
        box = GetComponentInParent<Box>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(box.type == Box.BoxType.Nitro)
        {
            if(collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy")
            {
                box.Explode(box.explosionRadius);
            }
        }
    }
}
