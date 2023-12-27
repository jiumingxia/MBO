using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct SharedComponent : ISharedComponentData
{
    public NativeArray<float3> position;
}
