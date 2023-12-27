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
public partial struct SelectItemSystem : ISystem
{
    private int mSelectItemCount;//当前选中物体计数

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int count = 0;
        //根据分组统计选中物体的数量,可选中物体的总数量
        NativeHashMap<int, int> groupEntitiesMap = new NativeHashMap<int, int>(6, Allocator.Temp);
        foreach (var itemSelect in SystemAPI.Query<RefRO<ItemSelectComponent>>())
        {
            if (itemSelect.ValueRO.waitDestroy == 0)
            {
                if (groupEntitiesMap.ContainsKey(itemSelect.ValueRO.groupId))
                {
                    groupEntitiesMap[itemSelect.ValueRO.groupId]++;
                }
                else
                {
                    groupEntitiesMap.Add(itemSelect.ValueRO.groupId, 1);
                }
                count++;
            }
        }
        //在当前选中物体计数发生变化时，重新设置选中物体的目标位置信息
        if (count != mSelectItemCount)
        {
            int indexOffset = 1;
            var entities = state.EntityManager.GetAllEntities();
            foreach (var entity in entities)
            {
                if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
                {
                    if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                    {
                        var ItemSelect = SystemAPI.GetComponentRW<ItemSelectComponent>(entity);
                        //判断当前物体不是等待销毁状态，同时目标id为0表示需要设置目标位置
                        if (ItemSelect.ValueRO.targetId == 0 && ItemSelect.ValueRO.waitDestroy == 0)
                        {
                            //该处需要禁用物理效果，时间有限没有找到合适的方法，仅将质量设置为0不受重力影响
                            var physicsMass = SystemAPI.GetComponentRW<PhysicsMass>(entity);
                            physicsMass.ValueRW.InverseMass = 0;
                            ItemSelect.ValueRW.targetId = mSelectItemCount + indexOffset;
                            indexOffset++;
                        }
                    }
                }
            }
            mSelectItemCount = count;
        }

        // 处理下计数等于7的情况，将物体重置
        if (mSelectItemCount == 7)
        {
            var entities = state.EntityManager.GetAllEntities();
            foreach (var entity in entities)
            {
                if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
                {
                    if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                    {
                        var ItemSelect = SystemAPI.GetComponentRW<ItemSelectComponent>(entity);
                        if (ItemSelect.ValueRO.waitDestroy == 0)
                        {
                            ItemSelect.ValueRW.targetId = 0;
                            ItemSelect.ValueRW.moveTotalTime = 0;
                            var physicsMass = SystemAPI.GetComponentRW<PhysicsMass>(entity);
                            physicsMass.ValueRW.InverseMass = 0.25f;
                            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                            transform.ValueRW.Scale = 1;
                            transform.ValueRW.Position = new float3(0, mSelectItemCount + 10, 0);
                            mSelectItemCount--;
                        }
                        state.EntityManager.SetComponentEnabled<ItemSelectComponent>(entity, false);
                    }
                }
            }
        }
        
        //通过分组的计数，标记数量达到3个需要被销毁的物体
        int isDestroyItem = 0;
        foreach (var value in groupEntitiesMap)
        {
            if (value.Value == 3)
            {
                var entities = state.EntityManager.GetAllEntities();
                foreach (var entity in entities)
                {
                    if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
                    {
                        if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                        {
                            var itemSelect = SystemAPI.GetComponentRW<ItemSelectComponent>(entity);
                            if (itemSelect.ValueRO.groupId == value.Key && itemSelect.ValueRO.waitDestroy == 0)
                            {
                                itemSelect.ValueRW.waitDestroy = 1;
                                mSelectItemCount = 0;
                                isDestroyItem = 1;
                            }
                        }
                    }
                }
            }
        }
        groupEntitiesMap.Dispose();

        //如果出现销毁的物体，需要重置其他选中物体的目标位置信息
        if (isDestroyItem == 1)
        {
            var entities = state.EntityManager.GetAllEntities();
            foreach (var entity in entities)
            {
                if (state.EntityManager.HasComponent<ItemSelectComponent>(entity))
                {
                    if (state.EntityManager.IsComponentEnabled<ItemSelectComponent>(entity))
                    {
                        var ItemSelect = SystemAPI.GetComponentRW<ItemSelectComponent>(entity);
                        if (ItemSelect.ValueRO.waitDestroy == 0)
                        {
                            ItemSelect.ValueRW.targetId = 0;
                            ItemSelect.ValueRW.moveFinish = 0;
                        }
                    }
                }
            }
        }
    }
}
