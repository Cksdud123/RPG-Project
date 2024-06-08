using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    NavMeshSurface Nav;

    private int CurrentEnemyCount;
    public int MaxEnemyCount = 5;

    private void Awake()
    {
        Nav = GetComponent<NavMeshSurface>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyCount = 0;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            RandomSpawner();
        }
    }
    private void RandomSpawner()
    {
        // 현재 위치에서 설정한 영역만큼 +-를 해서 랜덤으로 위치를 설정 
        Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-Nav.size.x / 2, Nav.size.x / 2), Nav.center.y, Random.Range(-Nav.size.z / 2, Nav.size.z / 2));
        var enemyDragon = ObjectPoolingManager.instance.GetGo("Dragon");
        enemyDragon.transform.position = randomSpawnPosition;
        enemyDragon.transform.rotation = Quaternion.identity;
    }
}
