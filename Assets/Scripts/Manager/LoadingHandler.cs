using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class LoadingHandler 
{

    //Classe dummy necessaria per la realizzazione di caricamenti async
    //Perchè non è possibile usare coroutine senza classi monobehaviour
    private class LoadingMonoBehaviour : MonoBehaviour {}

    //Enum contenente tutte le scene che possonno essere caricate;
    public enum Scene
    {
        LoadingScene,
        MainMenu,
        CharacterSelection,
        CasseScene
    };

    //Le action sono necessarie per la realizzazione di callback
    //creando "variabili" per conservare funzione che possono
    //essere usate come parametri di altre funzioni
    private static Action onLoadingCallback;

    public static void Load(Scene scene)
    {   
        //Impostiamo la callback per caricare la scena target
        onLoadingCallback = () => {
            //Versione rapida di tipo sync
            //SceneManager.LoadScene(scene.ToString());
            
            //Versione Async
            GameObject loadingGameObject = new GameObject("Loading");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));        
        };
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        //primo frame return
        yield return null;

        //Al secondo inizia l'operazione
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while(!asyncOperation.isDone)
        {
            //Attende il prossimo frame per continuare il while (?)
            yield return null;
        }
    }

    public static void LoadingCallback(){
        if(onLoadingCallback == null)
            return;

        onLoadingCallback();
        onLoadingCallback = null;
    }
}
