using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    NavMeshSurface Nav;

    public int MaxEnemyCount = 5;
    public int CurrentEnemyCount = 0;
    public string enemyName;
    public GameObject enemyParent;

    [HideInInspector] public int DeathCount;

    public Material[] materials;

    private void Awake()
    {
        Nav = GetComponent<NavMeshSurface>();
    }
    private void Start()
    {
        CurrentEnemyCount = 0;
    }
    // �ӹ��� ���϶�
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RandomSpawner();
        }
    }
    // ���ö�
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(enemyParent.transform.childCount);
            for (int i = 0; i < enemyParent.transform.childCount; i++)
            {
                GameObject enemy = enemyParent.transform.GetChild(i).gameObject;

                // �ڽ� ��ü�� �̸��� enemyName�� ��ġ�ϴ� ��쿡�� ��Ȱ��ȭ
                if (enemy.name.StartsWith(enemyName) && enemy.activeSelf)
                {
                    Enemy enemyDeactive = enemy.GetComponent<Enemy>();

                    StartCoroutine(enemyDeactive.Respawner(3f));
                }
            }
            CurrentEnemyCount = 0;
        }
    }
    private void RandomSpawner()
    {
        if (CurrentEnemyCount == MaxEnemyCount) return;

        // ���� ��ġ���� ������ ������ŭ +-�� �ؼ� �������� ��ġ�� ���� 
        Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-Nav.size.x / 2, Nav.size.x / 2), Nav.center.y, Random.Range(-Nav.size.z / 2, Nav.size.z / 2));
        var enemy = ObjectPoolingManager.instance.GetGo(enemyName);
        NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
        Rigidbody enemyRigid = enemy.GetComponent<Rigidbody>();
        Enemy enemyHP = enemy.GetComponent<Enemy>();

        // ������ ���׸��� ����
        Material randomMaterial = materials[Random.Range(0, materials.Length)];

        // ������Ʈ�� ������ ������Ʈ���� ���׸����� ����
        Renderer renderer = enemy.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material = randomMaterial;
        }

        // ������ ����
        enemyHP.InitializeHealth();
        enemyHP.SetSpawner(this);

        enemy.transform.position = randomSpawnPosition;
        enemy.transform.rotation = Quaternion.identity;
        enemy.GetComponent<Collider>().enabled = true;

        enemyRigid.isKinematic = false;

        enemyAgent.enabled = true;

        CurrentEnemyCount++;
    }
    public void DecrementEnemyCount()
    {
        CurrentEnemyCount = Mathf.Max(0, CurrentEnemyCount - 1);
        if (CurrentEnemyCount == 0) CurrentEnemyCount = MaxEnemyCount;
    }
}
