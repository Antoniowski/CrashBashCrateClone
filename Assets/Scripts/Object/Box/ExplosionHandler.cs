using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public int damage;

    public void HandleExplosion()
    {
        StartCoroutine(DeleteExplosionCollider());
    }

    IEnumerator DeleteExplosionCollider()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag)
            {
                case "Player":
                    collider.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
                    break;
                
                case "Enemy":
                    collider.gameObject.GetComponent<BotManager>().TakeDamage(damage);
                    break;
                case "Box":
                    collider.gameObject.GetComponent<Box>().Explode();
                    break;
                default:
                    print("Default Option");
                    break;
            }
    }
}
