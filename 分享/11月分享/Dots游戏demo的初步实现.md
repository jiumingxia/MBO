<h1 align=center>Dots小游戏初步实现</h1>

## 目录
- [目录](#目录)
- [一. 道具物品的创建](#一-道具物品的创建)
- [二. 射线实体创建](#二-射线实体创建)
- [三. 在输入的Dots系统中接收射线创建的实体，使用PhysicsWorldSingleton发射Dots下射线](#三-在输入的dots系统中接收射线创建的实体使用physicsworldsingleton发射dots下射线)
- [四. 当前工程链接地址](#四-当前工程链接地址)


## 一. 道具物品的创建

1. Authoring方式创建，绑定了一些预制体，设置了外部数量，通过System系统创建
   
   1. 创建Component数据

    ```
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
    ```

    2. 通过Authoring脚本，Bake生成道具所需实体

    
    ```
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
    ```
    3. 通过System来创建实体
    
    ```
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
    }
    ```
![Alt text](<屏幕录制2023-11-28 18.44.20.gif>)

## 二. 射线实体创建

   点击屏幕需要生成射线，要可以在Dots中使用需要同样创建成实体，将参数传递进去

 1. 创建Buffer数据
```
public struct MouseTouchInput : IBufferElementData
{
    public RaycastInput Value;
}
```
 2. 监听点击创建实体
```
void MouseClicked(InputAction.CallbackContext ctx)
{
    Vector2 screenPostion = ctx.ReadValue<Vector2>();
    Ray ray = Camera.ScreenPointToRay(screenPostion);
    
    Debug.Log(ray.GetPoint(Camera.farClipPlane));
    if (World.IsCreated && !World.EntityManager.Exists(Entity))
    {
        Entity = World.EntityManager.CreateEntity();
        World.EntityManager.AddBuffer<MouseTouchInput>(Entity);
    }
    RaycastInput input = new RaycastInput()
    {
        Start = ray.origin,
        Filter = CollisionFilter.Default,
        End = ray.GetPoint(Camera.farClipPlane)
    };
    World.EntityManager.GetBuffer<MouseTouchInput>(Entity).Add(new MouseTouchInput() { Value = input });
}
```

## 三. 在输入的Dots系统中接收射线创建的实体，使用PhysicsWorldSingleton发射Dots下射线

```
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        NativeArray<Entity> hitEntities = new NativeArray<Entity>(1, Allocator.Temp);
        foreach (var input in SystemAPI.Query<DynamicBuffer<MouseTouchInput>>())
        {
            foreach (var placementInput in input)
            {
                if (physicsWorld.CastRay(placementInput.Value, out var hit))
                {
                    Debug.Log($"{hit.Entity.ToString()}");
                    hitEntities[0] = hit.Entity;
                }
            }
            input.Clear();
        }
        
        state.EntityManager.DestroyEntity(hitEntities);
        hitEntities.Dispose();
    }
```

![Alt text](<屏幕录制2023-11-28 18.44.20-1.gif>)

## 四. 当前工程链接地址

   [https://github.com/jiumingxia/MBO/tree/main/DOTS](https://github.com/jiumingxia/MBO/tree/main/DOTS)