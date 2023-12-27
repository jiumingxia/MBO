using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SelectItemMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entities = state.EntityManager.GetAllEntities();
        foreach (var entity in entities)
        {
            if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
            {
                if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                {
                    var itemSelect = SystemAPI.GetComponentRW<ItemSelectComponent>(entity);
                    int targetId = itemSelect.ValueRO.targetId;
                    int isFinish = itemSelect.ValueRO.moveFinish;
                    //只控制目标位置不为零，切没有移动完成的实体
                    if (targetId != 0 && isFinish == 0)
                    {
                        //根据targetId设置目标位置
                        float3 targetPos = float3.zero;
                        var selectPos = SystemAPI.GetSingleton<GameCtrlSingletonComponent>();
                        switch (targetId)
                        {
                            case 1:
                                targetPos = selectPos.selectPos1;
                                break;
                            case 2:
                                targetPos = selectPos.selectPos2;
                                break;
                            case 3:
                                targetPos = selectPos.selectPos3;
                                break;
                            case 4:
                                targetPos = selectPos.selectPos4;
                                break;
                            case 5:
                                targetPos = selectPos.selectPos5;
                                break;
                            case 6:
                                targetPos = selectPos.selectPos6;
                                break;
                            case 7:
                                targetPos = selectPos.selectPos7;
                                break;
                        }

                        // 控制物体移动
                        var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                        if (itemSelect.ValueRO.moveTotalTime == 0)
                        {
                            // 设置移动相关的初始信息
                            itemSelect.ValueRW.moveTotalTime = 30;
                            itemSelect.ValueRW.moveCurrentTime = 1;
                            itemSelect.ValueRW.moveStartPos = transform.ValueRO.Position;
                            itemSelect.ValueRW.moveStartScale = transform.ValueRO.Scale;
                            itemSelect.ValueRW.moveStartRot = transform.ValueRO.Rotation;
                        }
                        else
                        {
                            // 移动帧数累加
                            itemSelect.ValueRW.moveCurrentTime++;
                        }

                        // 根据当前帧数计数移动比例，设置物体transform信息
                        float ratio = itemSelect.ValueRO.moveCurrentTime / (float)itemSelect.ValueRO.moveTotalTime;
                        transform.ValueRW.Position = math.lerp(itemSelect.ValueRO.moveStartPos, targetPos, ratio);
                        transform.ValueRW.Scale = math.lerp(itemSelect.ValueRO.moveStartScale, 0.2f, ratio);
                        transform.ValueRW.Rotation = math.nlerp(itemSelect.ValueRO.moveStartRot, quaternion.identity, ratio);
                        if (ratio == 1)
                        {
                            // 移动完成，标记移动结束，重置状态
                            itemSelect.ValueRW.moveFinish = 1;
                            itemSelect.ValueRW.moveTotalTime = 0;
                            transform.ValueRW.Position = targetPos;
                            transform.ValueRW.Scale = 0.2f;
                            transform.ValueRW.Rotation = quaternion.identity;
                            var velocity = SystemAPI.GetComponentRW<PhysicsVelocity>(entity);
                            velocity.ValueRW.Angular = float3.zero;
                            velocity.ValueRW.Linear = float3.zero;
                        }
                    }
                }
            }
        }
    }
}
