using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPrefabManager : MonoBehaviour
{
    public PrefabHolder[] characterArray;

    public GameObject GetCharacterController(bool humanPlayer, string charName)
    {
        PrefabHolder gameObject = Array.Find(characterArray, character => character.name == charName);
        if(humanPlayer)
            return gameObject.playerObject;
         
        return gameObject.CPUObject;
    }

    public GameObject GetModel(string charName)
    {
        PrefabHolder gameObject = Array.Find(characterArray, character => character.name == charName);
        return gameObject.model;
    }
}
