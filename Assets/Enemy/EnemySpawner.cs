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
    public string enemyName;

    public Material[] materials;
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
        // ���� ��ġ���� ������ ������ŭ +-�� �ؼ� �������� ��ġ�� ���� 
        Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-Nav.size.x / 2, Nav.size.x / 2), Nav.center.y, Random.Range(-Nav.size.z / 2, Nav.size.z / 2));
        var enemy = ObjectPoolingManager.instance.GetGo(enemyName);

        // ������ ���׸��� ����
        Material randomMaterial = materials[Random.Range(0, materials.Length)];

        // ������Ʈ�� ������ ������Ʈ���� ���׸����� ����
        Renderer renderer = enemy.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material = randomMaterial;
        }

        enemy.transform.position = randomSpawnPosition;
        enemy.transform.rotation = Quaternion.identity;
    }
}
