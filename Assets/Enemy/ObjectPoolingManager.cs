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
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject perfab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
        // 생성위치
        public Transform ObjectPos;
    }

    public static ObjectPoolingManager instance;

    // 오브젝트풀 매니저 준비 완료표시
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;
    private Transform Pos;
    private Material objMat;
    // 오브젝트를 담을 부모객체
    public Transform ObjectParent;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
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
            // 풀에 정보를 등록, 생성, 대여, 반환, 삭제,
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            // goDic에 현재 오브젝트의 이름과 프리팹을 저장
            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            // 오브젝트를 관리할 딕셔너리에도 저장
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                // 현재 인덱스에 있는 이름을 할당한 뒤에
                objectName = objectInfos[idx].objectName;
                Pos = objectInfos[idx].ObjectPos;
                // CreatePooledItem를 통해 생성해준뒤에
                Poolable poolAbleGo = CreatePooledItem().GetComponent<Poolable>();
                // 다시 반환해줌
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        IsReady = true;
    }
    // 생성
    private GameObject CreatePooledItem()
    {
        // 오브젝트 풀을 생성할 때 무작위 위치를 정합니다.
        Vector3 randomSpawnPosition = Pos.transform.position;
        GameObject poolGo = Instantiate(goDic[objectName],randomSpawnPosition, Quaternion.identity,ObjectParent);
        poolGo.GetComponent<Poolable>().Pool = ojbectPoolDic[objectName];
        return poolGo;
    }

    // 대여
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", goName);
            return null;
        }

        return ojbectPoolDic[goName].Get();
    }
}
