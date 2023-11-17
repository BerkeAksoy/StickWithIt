using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Ball", menuName = "Ball")]
public class BallSO : ScriptableObject
{
    public Sprite face;
    public float price;
    public string explanation;
    public string unlocksAt;
}
