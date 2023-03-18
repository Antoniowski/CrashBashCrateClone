using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public enum BoxType
    {
        Normal,
        TNT,
        Nitro
    }

    public bool isActive;
    public BoxType type;
    public bool isThrown;
    public bool isPicked;
    bool countdownStarted;
    public int damage;
    public float explosionRadius;

    //MOVEMENT
    [SerializeField] AnimationCurve boxSpeed;
    float movementTimer;

    [Header("VFX")]
    [SerializeField] ParticleSystem explosion;

    [Header("TNT section")]
    [SerializeField] Material TNTThree;
    [SerializeField] Material TNTTwo;
    [SerializeField] Material TNTOne;

    [Header("Audio")]
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip buzzerSound;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        isThrown = false;
        damage = 1;
        movementTimer = 1;
    }

    void OnCollisionEnter(Collision other)
    {

        if(isActive)
        {
            AttackedBoxBeheaviour(other);
        }
        else if(isThrown)
        {
            ThrownBoxBeheaviour(other);
        }
    }

    private void AttackedBoxBeheaviour(Collision other)
    {
        
        switch(other.gameObject.tag)
        {
            case "Player":
                Explode(explosionRadius);
                other.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
                break;
                
            case "Enemy":
                Explode(explosionRadius);
                other.gameObject.GetComponent<BotManager>().TakeDamage(damage);
                break;
            case "Box":
                Explode(explosionRadius);
                other.gameObject.GetComponent<Box>().Explode();
                break;
            default:
                print("Default Option");
                break;
            }
    }

    private void ThrownBoxBeheaviour(Collision other)
    {
        Explode(explosionRadius);
        switch(other.gameObject.tag)
        {
            case "Player":
                other.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
                break;
            case "Enemy":
                other.gameObject.GetComponent<BotManager>().TakeDamage(damage);
                break;
            case "Box":
                other.gameObject.GetComponent<Box>().Explode();
                break;
            case "Floor":
                break;
            default:
                break;
        }
    }

    private void TouchedBoxBeheaviour(Collision other)
    {
        if(type == BoxType.Nitro)
        {
            if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
                StartCoroutine(NitroExplosion());
        }
            
    }

    public void Explode(float radius = 2)
    {
        if(type == BoxType.TNT)
        {
            //TODO TNT
            if(isActive || isThrown || countdownStarted) goto Skip;
            StartCoroutine(TNTCountDown());
            return;
            Skip: /*Nothing*/;
                
        }
        Explosion(radius);
    }

    private void Explosion(float radius)
    {
        //VFX esplosione
        if(explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);
        
        //Suono
        try
        {
            int number = Mathf.RoundToInt(Random.Range(0,hitSounds.Length));
            AudioSource.PlayClipAtPoint(hitSounds[number], transform.position, 10);
        }catch{}

        //Onda d'urto
        GameObject esplosione = new GameObject("Onda d'urto");
        esplosione.transform.position = transform.position;
        SphereCollider raggioEsplosione = esplosione.AddComponent<SphereCollider>();
        raggioEsplosione.isTrigger = true;
        raggioEsplosione.radius = radius;
        ExplosionHandler ee = esplosione.AddComponent<ExplosionHandler>();
        ee.damage = damage;
        ee.HandleExplosion();

        //Rimozione oggetto
        gameObject.SetActive(false);
        if(!isActive) //Questo serve per evitare di interrompere delle azioni
            Destroy(gameObject);
    }

    public IEnumerator Move(Vector3 direction)
    {
        isActive = true;
        while(movementTimer > 0)
        {
            transform.position += direction*boxSpeed.Evaluate(movementTimer)*Time.deltaTime;
            movementTimer -= Time.deltaTime;
            yield return null;
        }

        movementTimer = 1;
        isActive = false;
        if(!gameObject.activeInHierarchy)
            Destroy(gameObject);
    }

    private IEnumerator TNTCountDown()
    {
        countdownStarted = true;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        float countdown = 0;
        while(countdown <= 2)
        {
            if(countdown == 0)
            {
                meshRenderer.material = TNTThree;
            }
            else if(countdown == 1)
            {
                meshRenderer.material = TNTTwo;
            }
            else
            {
                meshRenderer.material = TNTOne;
            }
            AudioSource.PlayClipAtPoint(buzzerSound, transform.position, 10);
            yield return new WaitForSeconds(1);
            countdown += 1;
        }

        Explosion(explosionRadius);
    }

    private IEnumerator NitroExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        Explosion(explosionRadius);
    }
}
