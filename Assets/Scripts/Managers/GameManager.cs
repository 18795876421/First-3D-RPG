using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStates playerState;

    public void RigisterPlayer(CharacterStates palyer)
    {
        playerState = palyer;
    }
}
