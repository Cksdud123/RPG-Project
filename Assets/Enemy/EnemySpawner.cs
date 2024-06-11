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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RandomSpawner();
        }
    }
    private void RandomSpawner()
    {
        if (CurrentEnemyCount == MaxEnemyCount) return;

        // 현재 위치에서 설정한 영역만큼 +-를 해서 랜덤으로 위치를 설정 
        Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-Nav.size.x / 2, Nav.size.x / 2), Nav.center.y, Random.Range(-Nav.size.z / 2, Nav.size.z / 2));
        var enemy = ObjectPoolingManager.instance.GetGo(enemyName);
        NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
        Rigidbody enemyRigid = enemy.GetComponent<Rigidbody>();
        Enemy enemyHP = enemy.GetComponent<Enemy>();

        // 랜덤한 메테리얼 선택
        Material randomMaterial = materials[Random.Range(0, materials.Length)];

        // 오브젝트의 렌더러 컴포넌트에서 메테리얼을 변경
        Renderer renderer = enemy.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material = randomMaterial;
        }

        enemyHP.InitializeHealth();
        // Enemy 클래스의 참조해서 
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
