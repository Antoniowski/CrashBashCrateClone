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
    [SerializeField] GameObject wumpa;

    float spawnPlane_xDim;
    float spawnPlane_zDim;
    float boxTimer;
    float wumpaTimer;


    [Header("Match Options")]
    [SerializeField] int maxNumberOfBox = 12;

    int numberOfBoxes = 0;
    [SerializeField] LayerMask boxLayer;

    

    public bool mostraComandi = true;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        matchManager = GameObject.Find("MatchHandler").GetComponent<MatchManager>();
        spawnPlane_xDim = spawnPlane.GetComponent<MeshRenderer>().bounds.size.x/2;
        spawnPlane_zDim = spawnPlane.GetComponent<MeshRenderer>().bounds.size.z/2;     
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManager.StopAllMusic();
        //Time.timeScale = 0;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        boxTimer += Time.deltaTime;
        wumpaTimer += Time.deltaTime;

        if(boxTimer > 6f)
        {
            boxTimer = 0;
            StartCoroutine(CrateSpawn(false, 0));
        }

        if(wumpaTimer > 10f)
        {
            wumpaTimer = 0;
            StartCoroutine(WumpaSpawn(0));
        }
    }

    void Init()
    {
        matchManager.timer = 90;
        mostraComandi = true;
        //Spawna i giocatori
        StartCoroutine(SpawnPlayers());
        //Spawna le prime casse
        StartCoroutine(CrateSpawn(true, 0));
    }

//Funziona
    IEnumerator SpawnPlayers()
    {
        GameObject gameObject = Instantiate(matchManager.player01.controller, spawnP1.transform.position, Quaternion.identity) as GameObject;
        GameObject gameObject2 = Instantiate(matchManager.player02.controller, spawnP2.transform.position, Quaternion.identity) as GameObject;
        GameObject gameObject3 = Instantiate(matchManager.player03.controller, spawnP3.transform.position, Quaternion.identity) as GameObject;
        GameObject gameObject4 = Instantiate(matchManager.player04.controller, spawnP4.transform.position, Quaternion.identity) as GameObject;
        yield return null;
    }
    //Funziona
    IEnumerator CrateSpawn(bool init, int spawnID)
    {
        //Randomizza la posizione
        float random_x = Random.Range(-spawnPlane_xDim, spawnPlane_xDim);
        float random_z = Random.Range(-spawnPlane_zDim, spawnPlane_zDim);
        random_z = random_x > random_z ? Random.Range(0, random_z) : Random.Range(0, random_x);

        //Crea nuova posizione
        //Vector3 newPos = new Vector3(Mathf.RoundToInt(spawnPlane.position.x + random_x*number*noise), 1f, Mathf.RoundToInt(spawnPlane.position.z + random_z*number*noise));
        Vector3 newPos = new Vector3(spawnPlane.position.x + random_x, 1f, spawnPlane.position.z + random_z);

        //Check se ci sta un'altra scatola
        if(Physics.OverlapSphere(newPos, 2f, boxLayer.value).Length == 0)
        {
            int randomIndex = Mathf.RoundToInt(Random.Range(0, boxes.Length));
            
            //Crea l'oggetto
            GameObject newBox = Instantiate(boxes[randomIndex], newPos, Quaternion.Euler(-90,0,0)) as GameObject;
            numberOfBoxes += 1;

        }
        //INIT
        if(init){
            if(numberOfBoxes < maxNumberOfBox)
                StartCoroutine(CrateSpawn(true, 0));
        }
        else //SPAWN GENERICO
        {
            if(spawnID != 5)
                StartCoroutine(CrateSpawn(false, spawnID+1));
            
        }
        

        yield return null;
    }
    
    IEnumerator WumpaSpawn(int spawnID)
    {
                //Randomizza la posizione
        float random_x = Random.Range(-spawnPlane_xDim, spawnPlane_xDim);
        float random_z = Random.Range(-spawnPlane_zDim, spawnPlane_zDim);
        random_z = random_x > random_z ? Random.Range(0, random_z) : Random.Range(0, random_x);

        //Crea nuova posizione
        //Vector3 newPos = new Vector3(Mathf.RoundToInt(spawnPlane.position.x + random_x*number*noise), 1f, Mathf.RoundToInt(spawnPlane.position.z + random_z*number*noise));
        Vector3 newPos = new Vector3(spawnPlane.position.x + random_x, 1f, spawnPlane.position.z + random_z);

        //Check se ci sta un'altra scatola
        if(Physics.OverlapSphere(newPos, 1f, boxLayer.value).Length == 0)
        {            
            //Crea l'oggetto
            GameObject newBox = Instantiate(wumpa, newPos, Quaternion.Euler(0,0,0)) as GameObject;
        }

        int numberOfWumpa = Mathf.RoundToInt(Random.Range(2, 3));
        if(spawnID != numberOfWumpa)
            StartCoroutine(WumpaSpawn(spawnID+1));

        yield return null;
    }
}
