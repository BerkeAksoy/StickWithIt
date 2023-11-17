using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Level", menuName = "Level")]
public class LevelSO : ScriptableObject
{
    [Header("Easy / Medium / Hard")]

    public int[] allowedJumpCount;


}
