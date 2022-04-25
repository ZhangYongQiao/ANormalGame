using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="CreateAIBaseInfoAsset")]
public class AIBaseInfo : ScriptableObject
{
    public Vector2 xAxis;
    public Vector2 zAxis;
    public float yAxis;

    public List<AnimationClip> clips;
}
