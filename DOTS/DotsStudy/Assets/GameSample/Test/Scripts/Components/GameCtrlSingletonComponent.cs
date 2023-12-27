using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct GameCtrlSingletonComponent : IComponentData
{
    public float3 selectPos1;
    public float3 selectPos2;
    public float3 selectPos3;
    public float3 selectPos4;
    public float3 selectPos5;
    public float3 selectPos6;
    public float3 selectPos7;
    public int gameState;
}
