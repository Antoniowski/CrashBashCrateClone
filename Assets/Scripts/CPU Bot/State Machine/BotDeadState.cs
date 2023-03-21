using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDeadState : State
{
    public override State RunCurrentState()
    {
        return this;
    }
}
