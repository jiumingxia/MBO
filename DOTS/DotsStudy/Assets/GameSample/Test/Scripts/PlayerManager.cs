using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using Ray = UnityEngine.Ray;

public class PlayerManager : MonoBehaviour
{
    public InputAction Input;
    public Camera Camera;

    private Entity Entity; 
    private World World;

    private void OnEnable()
    {
        Input.started += MouseClicked;
        Input.Enable();

        Camera = Camera == null ? Camera.main : Camera;

        World = World.DefaultGameObjectInjectionWorld;
    }

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

    private void OnDisable()
    {
        Input.started -= MouseClicked;
        Input.Disable();

        if (World.IsCreated && !World.EntityManager.Exists(Entity))
        {
            World.EntityManager.DestroyEntity(Entity);
        }
    }
}

public struct MouseTouchInput : IBufferElementData
{
    public RaycastInput Value;
}
