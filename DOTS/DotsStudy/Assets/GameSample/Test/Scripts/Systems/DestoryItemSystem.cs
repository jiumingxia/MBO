using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct DestoryItemSystem : ISystem
{
    
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 按分类统计待销毁且移动完成的物体
        NativeHashMap<int, int> moveFinishMap = new NativeHashMap<int, int>(6, Allocator.Temp);
        var entities = state.EntityManager.GetAllEntities();
        foreach (var entity in entities)
        {
            if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
            {
                if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                {
                    var ItemSelect = SystemAPI.GetComponentRW<ItemSelectComponent>(entity);
                    if (ItemSelect.ValueRO.waitDestroy == 1 && ItemSelect.ValueRO.moveFinish == 1)
                    {
                        if (moveFinishMap.ContainsKey(ItemSelect.ValueRO.groupId))
                        {
                            moveFinishMap[ItemSelect.ValueRO.groupId]++;
                        }
                        else
                        {
                            moveFinishMap.Add(ItemSelect.ValueRO.groupId, 1);
                        }
                    }
                }
            }
        }
        foreach (var data in moveFinishMap)
        {
            if (data.Value >= 3)
            {
                // 这里控制销毁的物体数量是3的倍数
                int destroyCount = data.Value / 3 * 3;
                NativeArray<Entity> destroyArr = new NativeArray<Entity>(destroyCount, Allocator.Temp);
                foreach (var entity in entities)
                {
                    if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
                    {
                        if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                        {
                            var itemSelect = state.EntityManager.GetComponentData<ItemSelectComponent>(entity);
                            if (itemSelect.waitDestroy == 1 && itemSelect.moveFinish == 1 && itemSelect.groupId == data.Key)
                            {
                                // 可销毁的物体添加到待销毁的临时数组中
                                destroyArr[destroyArr.Length - destroyCount] = entity;
                                destroyCount--;
                                if (destroyCount == 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                // 销毁获取的物体数组
                state.EntityManager.DestroyEntity(destroyArr);
                destroyArr.Dispose();
            }
        }
        moveFinishMap.Dispose();
    }
}
