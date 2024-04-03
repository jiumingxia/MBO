using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePauseCtrl : MonoBehaviour
{
    public Animator anim;
    public Transform circle;
    
    private Material circleMat;
    private bool isPause = false;
    private Coroutine mCircleCoroutine;

    private void Awake()
    {
        circleMat = circle.GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        circleMat.SetFloat("_Scale", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPause = !isPause;
            if (mCircleCoroutine != null)
            {
                StopCoroutine(mCircleCoroutine);
            }
            if (isPause)
            {
                anim.speed = 0;
                mCircleCoroutine = StartCoroutine(CircleScaleChange(1));
            }
            else
            {
                mCircleCoroutine = StartCoroutine(CircleScaleChange(0));
            }
        }
    }

    /// <summary>
    /// 控制模型所有顶点缩放动画
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator CircleScaleChange(float target)
    {
        yield return null;
        while (true)
        {
            float scale = circleMat.GetFloat("_Scale");
            float nextScale = Mathf.MoveTowards(scale, target, 0.005f);
            circleMat.SetFloat("_Scale", nextScale);
            if (Math.Abs(nextScale - target) < 0.001f)
            {
                break;
            }
            yield return null;
        }
        if (target == 0)
        {
            anim.speed = 1;
            circleMat.SetFloat("_Scale", 0);
        }
    }
}
