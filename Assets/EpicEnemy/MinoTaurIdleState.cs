using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinoTaurIdleState : StateMachineBehaviour
{
    private float idleRange = 3;
    private float attackCooldown; // ���� ��ٿ� �ð�

    float timer;
    float attackTimer; // ���� ��ٿ��� �����ϴ� Ÿ�̸�

    Transform player;
    int attackIndex;

    NavMeshAgent agent;
    EnemyEnterArea enemyEnterArea;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        attackTimer = 0; // �ʱ�ȭ
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        enemyEnterArea = FindObjectOfType<EnemyEnterArea>();
        attackIndex = Random.Range(1, 4);

        attackCooldown = Random.Range(1.0f, 2.0f);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (enemyEnterArea.isEnterArea)
        {
            timer += Time.deltaTime;
            attackTimer += Time.deltaTime; // ���� Ÿ�̸� ����

            // Walk ���·� ��ȯ
            if (timer > Random.Range(3.0f, 7.0f))
            {
                animator.SetBool("Walk", true);
            }
            // ����
            else if (distance < idleRange && attackTimer >= attackCooldown)
            {
                attackIndex = Random.Range(1, 4);
                animator.SetTrigger("Attack" + attackIndex);
                attackTimer = 0;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack" + attackIndex);
    }
}
