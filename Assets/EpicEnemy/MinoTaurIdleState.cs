using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinoTaurIdleState : StateMachineBehaviour
{
    private float idleRange = 3;

    float timer;

    Transform player;
    int attackIndex;

    NavMeshAgent agent;
    EnemyEnterArea enemyEnterArea;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        enemyEnterArea = FindObjectOfType<EnemyEnterArea>();
        attackIndex = Random.Range(1, 4);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (enemyEnterArea.isEnterArea)
        {
            timer += Time.deltaTime;

            // Walk 상태로 전환
            if (timer > Random.Range(3.0f, 7.0f))
            {
                animator.SetBool("Walk", true);
            }
            // 공격
            else if (distance < idleRange)
            {
                agent.isStopped = true;
                animator.SetTrigger("Attack" + attackIndex);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack" + attackIndex);
    }
}
