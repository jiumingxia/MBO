using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[BurstCompile]
public partial struct InputSystem : ISystem
{
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        // NativeArray<Entity> hitEntities = new NativeArray<Entity>(1, Allocator.Temp);
        foreach (var input in SystemAPI.Query<DynamicBuffer<MouseTouchInput>>())
        {
            foreach (var placementInput in input)
            {
                if (physicsWorld.CastRay(placementInput.Value, out var hit))
                {
                    if (!state.EntityManager.HasComponent<FloorTagComponent>(hit.Entity))
                    {
                        // hitEntities[0] = hit.Entity;
                        if (state.EntityManager.HasComponent<ItemSelectComponent>(hit.Entity) 
                            && !state.EntityManager.IsComponentEnabled<ItemSelectComponent>(hit.Entity))
                        {
                            state.EntityManager.SetComponentEnabled<ItemSelectComponent>(hit.Entity, true);
                        }
                    }
                }
            }
            input.Clear();
        }
        
        // state.EntityManager.DestroyEntity(hitEntities);
        // hitEntities.Dispose();
    }
}
