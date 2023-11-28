using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
public partial struct GeneratePhysicsBodySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MultiPhysicsBodyGenerator>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var generator = SystemAPI.GetSingleton<MultiPhysicsBodyGenerator>();

        var itemCubes01 = CollectionHelper.CreateNativeArray<Entity>(
            (int)(generator.itemGridNums.x * generator.itemGridNums.y * generator.itemGridNums.z), Allocator.Temp);
        state.EntityManager.Instantiate(generator.item01EntityProtoTypes, itemCubes01);

        int count = 0;
        foreach (var cube in itemCubes01)
        {
            int y = count / (int)(generator.itemGridNums.x * generator.itemGridNums.z) + 50;
            int temp = count % (int)(generator.itemGridNums.x * generator.itemGridNums.z);
            int z = temp / (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            int x = temp % (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            var position = new float3(x, y, z) * generator.space;
            var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
            transform.ValueRW.Position = position;
            count++;
        }

        itemCubes01.Dispose();

        itemCubes01 = CollectionHelper.CreateNativeArray<Entity>(
            (int)(generator.itemGridNums.x * generator.itemGridNums.y * generator.itemGridNums.z), Allocator.Temp);
        state.EntityManager.Instantiate(generator.item02EntityProtoTypes, itemCubes01);

        count = 0;
        foreach (var cube in itemCubes01)
        {
            int y = count / (int)(generator.itemGridNums.x * generator.itemGridNums.z) + 50;
            int temp = count % (int)(generator.itemGridNums.x * generator.itemGridNums.z);
            int z = temp / (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            int x = temp % (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            var position = new float3(x, y, z) * generator.space;
            var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
            transform.ValueRW.Position = position;
            count++;
        }

        itemCubes01.Dispose();

        itemCubes01 = CollectionHelper.CreateNativeArray<Entity>(
            (int)(generator.itemGridNums.x * generator.itemGridNums.y * generator.itemGridNums.z), Allocator.Temp);
        state.EntityManager.Instantiate(generator.item03EntityProtoTypes, itemCubes01);

        count = 0;
        foreach (var cube in itemCubes01)
        {
            int y = count / (int)(generator.itemGridNums.x * generator.itemGridNums.z) + 50;
            int temp = count % (int)(generator.itemGridNums.x * generator.itemGridNums.z);
            int z = temp / (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            int x = temp % (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            var position = new float3(x, y, z) * generator.space;
            var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
            transform.ValueRW.Position = position;
            count++;
        }

        itemCubes01.Dispose();


        itemCubes01 = CollectionHelper.CreateNativeArray<Entity>(
            (int)(generator.itemGridNums.x * generator.itemGridNums.y * generator.itemGridNums.z), Allocator.Temp);
        state.EntityManager.Instantiate(generator.item04EntityProtoTypes, itemCubes01);

        count = 0;
        foreach (var cube in itemCubes01)
        {
            int y = count / (int)(generator.itemGridNums.x * generator.itemGridNums.z) + 50;
            int temp = count % (int)(generator.itemGridNums.x * generator.itemGridNums.z);
            int z = temp / (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            int x = temp % (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            var position = new float3(x, y, z) * generator.space;
            var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
            transform.ValueRW.Position = position;
            count++;
        }

        itemCubes01.Dispose();

        itemCubes01 = CollectionHelper.CreateNativeArray<Entity>(
            (int)(generator.itemGridNums.x * generator.itemGridNums.y * generator.itemGridNums.z), Allocator.Temp);
        state.EntityManager.Instantiate(generator.item05EntityProtoTypes, itemCubes01);

        count = 0;
        foreach (var cube in itemCubes01)
        {
            int y = count / (int)(generator.itemGridNums.x * generator.itemGridNums.z) + 50;
            int temp = count % (int)(generator.itemGridNums.x * generator.itemGridNums.z);
            int z = temp / (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            int x = temp % (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            var position = new float3(x, y, z) * generator.space;
            var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
            transform.ValueRW.Position = position;
            count++;
        }

        itemCubes01.Dispose();

        itemCubes01 = CollectionHelper.CreateNativeArray<Entity>(
            (int)(generator.itemGridNums.x * generator.itemGridNums.y * generator.itemGridNums.z), Allocator.Temp);
        state.EntityManager.Instantiate(generator.item06EntityProtoTypes, itemCubes01);

        count = 0;
        foreach (var cube in itemCubes01)
        {
            int y = count / (int)(generator.itemGridNums.x * generator.itemGridNums.z) + 50;
            int temp = count % (int)(generator.itemGridNums.x * generator.itemGridNums.z);
            int z = temp / (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            int x = temp % (int)(generator.itemGridNums.x) - (int)generator.itemGridNums.z;
            var position = new float3(x, y, z) * generator.space;
            var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
            transform.ValueRW.Position = position;
            count++;
        }

        itemCubes01.Dispose();

        state.Enabled = false;
    }
}
