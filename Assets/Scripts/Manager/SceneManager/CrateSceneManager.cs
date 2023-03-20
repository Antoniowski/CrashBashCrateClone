using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSceneManager : MonoBehaviour
{
    AudioManager audioManager;
    MatchManager matchManager;
    
    [Header("Spawn Points")]
    [SerializeField] GameObject spawnP1;
    [SerializeField] GameObject spawnP2;
    [SerializeField] GameObject spawnP3;
    [SerializeField] GameObject spawnP4;

    [Header("Spawn Element")]
    [SerializeField] Transform spawnPlane;
    [SerializeField] Transform centerRing;
    [SerializeField] GameObject[] boxes;


    [Header("Match Options")]
    [SerializeField] int maxNumberOfBox = 12;

    int numberOfBoxes = 0;
    List<Vector3> positionTaken = new List<Vector3>();
    [SerializeField] LayerMask boxLayer;

  

    public bool mostraComandi = true;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        matchManager = GameObject.Find("MatchHandler").GetComponent<MatchManager>();        
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManager.StopAllMusic();
        //Time.timeScale = 0;
        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(CheckBoxes());
    }

    IEnumerator Init()
    {
        matchManager.timer = 90;
        mostraComandi = true;
        StartCoroutine(SpawnPlayers());
        StartCoroutine(CrateSpawn());
        yield return null;
    }


    IEnumerator SpawnPlayers()
    {
        GameObject gameObject = Instantiate(matchManager.player01.controller, spawnP1.transform.position, Quaternion.identity) as GameObject;
        GameObject gameObject2 = Instantiate(matchManager.player02.controller, spawnP2.transform.position, Quaternion.identity) as GameObject;
        GameObject gameObject3 = Instantiate(matchManager.player03.controller, spawnP3.transform.position, Quaternion.identity) as GameObject;
        GameObject gameObject4 = Instantiate(matchManager.player04.controller, spawnP4.transform.position, Quaternion.identity) as GameObject;
        yield return null;
    }

    IEnumerator CrateSpawn()
    {
        //Ottieni dimensioni del piano
        float x_dim = spawnPlane.GetComponent<MeshRenderer>().bounds.size.x;
        float z_dim = spawnPlane.GetComponent<MeshRenderer>().bounds.size.z;
        x_dim = x_dim/2;
        z_dim = z_dim/2;

    
        //while(numberOfBoxes < 8)
        //{
            numberOfBoxes += 1;
            int index = Mathf.RoundToInt(Random.Range(0, boxes.Length));
            StartCoroutine(SingleSpawn(boxes[index], x_dim, z_dim));
            yield return null;
        //}

    }

    IEnumerator SingleSpawn(GameObject box, float x, float z)
    {
        //Crea l'oggetto
        GameObject newBox = Instantiate(box, Vector3.zero, Quaternion.Euler(-90,0,0)) as GameObject;

        

        //Randomizza la posizione
        float random_x = Random.Range(-x, x);
        float random_z = Random.Range(-z, z);
        float number = Random.Range(1,1.2f);
        random_z = random_x > random_z ? Random.Range(0, random_z) : Random.Range(0, random_x);

        float noise = Mathf.PerlinNoise(random_x, random_z);

        //Assegna nuova posizione
        Vector3 newPos = new Vector3(Mathf.RoundToInt(spawnPlane.position.x + random_x*number*noise), 1f, Mathf.RoundToInt(spawnPlane.position.z + random_z*number*noise));
        foreach (Vector3 item in positionTaken)
        {
            if(newPos == item)
            {
                Destroy(newBox);
                int index = Mathf.RoundToInt(Random.Range(0, boxes.Length));
                StartCoroutine(SingleSpawn(boxes[index], x, z));
                yield break;
            }
                
        }

        if(Physics.CheckSphere(newPos, 5, boxLayer))
        {
            Destroy(newBox);
            int index = Mathf.RoundToInt(Random.Range(0, boxes.Length));
            StartCoroutine(SingleSpawn(boxes[index], x, z));
            yield break;
        }

        newBox.transform.position = newPos;
        positionTaken.Add(newPos);


        //Rimuovi dal piano
        newBox.transform.parent = null;
        
        numberOfBoxes += 1;
        if(numberOfBoxes < maxNumberOfBox)
        {    
            int index = Mathf.RoundToInt(Random.Range(0, boxes.Length));
            StartCoroutine(SingleSpawn(boxes[index], x, z));
        }
            
        
        yield return null;


    }

    IEnumerator CheckBoxes()
    {
        Collider[] colliders = Physics.OverlapBox(centerRing.position, new Vector3(10f,10f,10f), Quaternion.identity, boxLayer);
        if(colliders.Length <= maxNumberOfBox - 5)
        {
            float x_dim = spawnPlane.GetComponent<MeshRenderer>().bounds.size.x;
            float z_dim = spawnPlane.GetComponent<MeshRenderer>().bounds.size.z;
            StartCoroutine(SingleSpawn(boxes[Mathf.RoundToInt(Random.Range(0,boxes.Length))], x_dim, z_dim));
        }
        yield return null;
    }

    void WumpaSpawn()
    {

    }
}
