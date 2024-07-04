using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    private bool isReleased = false;

    // �����ʿ��� ������ �޾� ó��
    public void ReleaseObject()
    {
        if (!isReleased)
        {
            Pool.Release(gameObject);
            isReleased = true;
        }
        else
        {
            Debug.LogWarning("Trying to release an object that has already been released.");
        }
    }

    private void OnEnable()
    {
        isReleased = false;
    }
}
