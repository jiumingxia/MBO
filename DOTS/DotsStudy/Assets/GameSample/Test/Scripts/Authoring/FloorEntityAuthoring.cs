using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FloorEntityAuthoring : MonoBehaviour
{
    class Baker : Baker<FloorEntityAuthoring>
    {
        public override void Bake(FloorEntityAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale);
            AddComponent(entity, new FloorTagComponent());
        }
    }
}
