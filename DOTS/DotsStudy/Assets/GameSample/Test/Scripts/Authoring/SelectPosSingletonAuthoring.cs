using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SelectPosSingletonAuthoring : MonoBehaviour
{
    public Transform[] selectPosTransArr;
    
    public static SelectPosSingletonAuthoring instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    class Baker : Baker<SelectPosSingletonAuthoring>
    {
        public override void Bake(SelectPosSingletonAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var data = new GameCtrlSingletonComponent();
            data.selectPos1 = authoring.selectPosTransArr[0].position;
            data.selectPos2 = authoring.selectPosTransArr[1].position;
            data.selectPos3 = authoring.selectPosTransArr[2].position;
            data.selectPos4 = authoring.selectPosTransArr[3].position;
            data.selectPos5 = authoring.selectPosTransArr[4].position;
            data.selectPos6 = authoring.selectPosTransArr[5].position;
            data.selectPos7 = authoring.selectPosTransArr[6].position;
            AddComponent(entity, data);
        }
    }
}
