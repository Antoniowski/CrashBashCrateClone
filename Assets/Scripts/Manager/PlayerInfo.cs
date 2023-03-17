using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo 
{

    public enum ControllType
    {
        Keyboard,
        Gamepad
    }

    public int ID;
    public bool isCPU;
    public ControllType controllType;
    public  string character;

    public PlayerInfo(int ID, bool isCPU, ControllType controllType, string character)
    {
        this.ID = ID;
        this.isCPU = isCPU;
        this.controllType = controllType;
        this.character = character;
    }
}
