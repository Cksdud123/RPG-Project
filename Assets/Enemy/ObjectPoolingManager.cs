using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // ������Ʈ �̸�
        public string objectName;
        // ������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject perfab;
        // ��� �̸� ���� �س�������
        public int count;
        // ������ġ
        public Transform ObjectPos;
    }

    public static ObjectPoolingManager instance;

    // ������ƮǮ �Ŵ��� �غ� �Ϸ�ǥ��
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // ������ ������Ʈ�� key�������� ���� ����
    private string objectName;
    private Transform Pos;
    private Material objMat;
    // ������Ʈ�� ���� �θ�ü
    public Transform ObjectParent;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // ������ƮǮ���� ������Ʈ�� ���� �����Ҷ� ����� ��ųʸ�
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }
    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            // Ǯ�� ������ ���, ����, �뿩, ��ȯ, ����,
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            // goDic�� ���� ������Ʈ�� �̸��� �������� ����
            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            // ������Ʈ�� ������ ��ųʸ����� ����
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                // ���� �ε����� �ִ� �̸��� �Ҵ��� �ڿ�
                objectName = objectInfos[idx].objectName;
                Pos = objectInfos[idx].ObjectPos;
                // CreatePooledItem�� ���� �������صڿ�
                Poolable poolAbleGo = CreatePooledItem().GetComponent<Poolable>();
                // �ٽ� ��ȯ����
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        IsReady = true;
    }
    // ����
    private GameObject CreatePooledItem()
    {
        // ������Ʈ Ǯ�� ������ �� ������ ��ġ�� ���մϴ�.
        Vector3 randomSpawnPosition = Pos.transform.position;
        GameObject poolGo = Instantiate(goDic[objectName],randomSpawnPosition, Quaternion.identity,ObjectParent);
        poolGo.GetComponent<Poolable>().Pool = ojbectPoolDic[objectName];
        return poolGo;
    }

    // �뿩
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", goName);
            return null;
        }

        return ojbectPoolDic[goName].Get();
    }
}
