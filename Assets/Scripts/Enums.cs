using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum PlayerState { Ready, Selected, Busy, Attack }
    public enum EnemyState { Ready, EnableToAttack, Busy, Attack }
    public enum Unit { Player, Enemy }

}
