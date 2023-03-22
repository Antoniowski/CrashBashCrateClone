using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wumpa : MonoBehaviour
{

    AudioManager audioManager;

    [SerializeField] AudioClip wumpaSound;
    [SerializeField] int healthToRestore;

    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }
        catch
        {
            Debug.Log("Audio Manager non presente");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag.Equals("Player"))
        {
            AudioSource.PlayClipAtPoint(wumpaSound, transform.position, 1f);
            PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();
            player.SetCurrentHealth(player.health+healthToRestore);
            Destroy(gameObject);
        }

        if(collider.gameObject.tag.Equals("Enemy"))
        {
            AudioSource.PlayClipAtPoint(wumpaSound, transform.position, 1f);
            BotManager bot = collider.gameObject.GetComponent<BotManager>();
            bot.SetCurrentHealth(bot.health+healthToRestore);
            Destroy(gameObject);
        }
    }
}
