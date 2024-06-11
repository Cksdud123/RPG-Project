using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    // 스포너에서 갯수를 받아 처리
    public void ReleaseObject()
    {
        Pool.Release(gameObject);
    }
}
