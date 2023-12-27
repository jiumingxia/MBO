using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ItemSelectComponent : IComponentData , IEnableableComponent
{
    public int targetId;            // 收集的目标位置ID
    public int groupId;             // 道具分组，长得一样的道具为一组
    public int moveFinish;          // 是否位移完成
    public int waitDestroy;         // 是否处于等待销毁状态
    public int moveTotalTime;       // 移动的总时间（帧数）
    public int moveCurrentTime;     // 移动的当前帧数
    public float3 moveStartPos;     // 移动开始时的位置
    public float moveStartScale;    // 移动开始时的缩放
    public quaternion moveStartRot; // 移动开始时的旋转
}
