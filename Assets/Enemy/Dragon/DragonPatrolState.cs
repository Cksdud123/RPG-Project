using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonPatrolState : StateMachineBehaviour
{
    float chaseRange = 8f;
    float patrolRadius = 20f;
    float maxPatrolTime = 10f;

    NavMeshAgent agent;
    Transform player;

    public float updateInterval = 5f;
    private float timeSinceLastUpdate;
    private float totalTimeInState;

    // OnStateEnter�� ���� ���̰� ���۵ǰ� ���� �ӽ��� �� ���¸� ���ϱ� ������ �� ȣ��˴ϴ�.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        // Ÿ�̸� �ʱ�ȭ
        timeSinceLastUpdate = updateInterval;
        totalTimeInState = 0f;

        agent.speed = 1.5f;
    }

    // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ���̿� �ִ� �� ������Ʈ �����ӿ��� ȣ��˴ϴ�.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ÿ�̸� ������Ʈ
        timeSinceLastUpdate += Time.deltaTime;
        totalTimeInState += Time.deltaTime;

        // ���� �������� ������Ʈ�ؾ� �ϴ��� Ȯ��
        if (timeSinceLastUpdate >= updateInterval)
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            agent.SetDestination(randomPosition);
            timeSinceLastUpdate = 0;
        }

        // ������ �����ؾ� �ϴ��� Ȯ��
        if (totalTimeInState > maxPatrolTime)
        {
            animator.SetBool("isPatrolling", false);
        }

        // �÷��̾ �߰��ؾ� �ϴ��� Ȯ��
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit�� ���� ���̰� ������ ���� �ӽ��� �� ���� �򰡸� �Ϸ��� �� ȣ��˴ϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // ���� �ݰ� ���� ���� ����
        randomDirection += agent.transform.position; // ���� ������ ���� ��ġ�� ���մϴ�.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas)) // ��ġ�� NavMesh ���� �ִ��� Ȯ��
        {
            return hit.position; // ��ȿ�� NavMesh ��ġ�� ��ȯ
        }
        else
        {
            return agent.transform.position; // ��ȿ�� NavMesh ��ġ�� ã�� ���� ��� ���� ��ġ�� ��ȯ
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
