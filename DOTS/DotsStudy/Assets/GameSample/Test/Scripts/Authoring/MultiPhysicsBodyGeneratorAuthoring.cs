using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


struct MultiPhysicsBodyGenerator : IComponentData
{
    public Entity item01EntityProtoTypes;
    public Entity item02EntityProtoTypes;
    public Entity item03EntityProtoTypes;
    public Entity item04EntityProtoTypes;
    public Entity item05EntityProtoTypes;
    public Entity item06EntityProtoTypes;
    public float3 itemGridNums;
    public float space;
}

public class MultiPhysicsBodyGeneratorAuthoring : MonoBehaviour
{
    public GameObject[] cubePrefab = new GameObject[3];
    public Vector3 gridNum;
    public float space = 1.1f;

    public class Baker : Baker<MultiPhysicsBodyGeneratorAuthoring>
    {
        public override void Bake(MultiPhysicsBodyGeneratorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var data = new MultiPhysicsBodyGenerator();
            data.item01EntityProtoTypes = GetEntity(authoring.cubePrefab[0], TransformUsageFlags.None);
            data.item02EntityProtoTypes = GetEntity(authoring.cubePrefab[1], TransformUsageFlags.None);
            data.item03EntityProtoTypes = GetEntity(authoring.cubePrefab[2], TransformUsageFlags.None);
            data.item04EntityProtoTypes = GetEntity(authoring.cubePrefab[3], TransformUsageFlags.None);
            data.item05EntityProtoTypes = GetEntity(authoring.cubePrefab[4], TransformUsageFlags.None);
            data.item06EntityProtoTypes = GetEntity(authoring.cubePrefab[5], TransformUsageFlags.None);
            data.itemGridNums = authoring.gridNum;
            data.space = authoring.space;
            AddComponent(entity, data);
        }
    }
}
