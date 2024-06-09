using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonPatrolState : StateMachineBehaviour
{
    float chaseRange = 8f;
    float patrolRadius = 20f;
    float maxPatrolTime = 10f;

    NavMeshAgent agent;
    Transform player;

    public float updateInterval = 5f;
    private float timeSinceLastUpdate;
    private float totalTimeInState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        // 타이머 초기화
        timeSinceLastUpdate = updateInterval;
        totalTimeInState = 0f;

        agent.speed = 1.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 타이머 업데이트
        timeSinceLastUpdate += Time.deltaTime;
        totalTimeInState += Time.deltaTime;

        // 순찰 목적지를 업데이트해야 하는지 확인
        if (timeSinceLastUpdate >= updateInterval)
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            agent.SetDestination(randomPosition);
            timeSinceLastUpdate = 0;
        }

        // 순찰을 중지해야 하는지 확인
        if (totalTimeInState > maxPatrolTime)
        {
            animator.SetBool("isPatrolling", false);
        }

        // 플레이어를 추격해야 하는지 확인
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // 순찰 반경 내의 랜덤 방향
        randomDirection += agent.transform.position; // 랜덤 방향을 현재 위치에 더합니다.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas)) // 위치가 NavMesh 위에 있는지 확인
        {
            return hit.position; // 유효한 NavMesh 위치를 반환
        }
        else
        {
            return agent.transform.position; // 유효한 NavMesh 위치를 찾지 못한 경우 현재 위치를 반환
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
