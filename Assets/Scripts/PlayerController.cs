using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Third Person Controller References
    [SerializeField]
    private Animator playerAnim;
    private ThirdPersonController thirdPersonController;
    private CharacterController characterController;

    //��� �Ķ����
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;
    public bool isEquipped;

    //����
    public bool isBlocking;

    //������
    public bool isKicking;

    //����
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;

    // ����
    [SerializeField] AnimationCurve dodgeCurve;
    //ȸ��
    public bool isDodgeing;

    float dodgeTimer, dodge_coolDown;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        Attack();

        Equip();
        Block();
        Kick();
        Dodge();
    }
    private void Equip()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerAnim.GetBool("Grounded"))
        {
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
        }
    }

    public void ActiveWeapon()
    {
        if (!isEquipped)
        {
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
        }
        else
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
        }
    }

    public void Equipped()
    {
        isEquipping = false;
    }

    private void Block()
    {
        if (Input.GetKey(KeyCode.Mouse1) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Block", true);
            isBlocking = true;
        }
        else
        {
            playerAnim.SetBool("Block", false);
            isBlocking = false;
        }
    }

    public void Kick()
    {
        if (Input.GetKey(KeyCode.LeftControl) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Kick", true);
            isKicking = true;
        }
        else
        {
            playerAnim.SetBool("Kick", false);
            isKicking = false;
        }
    }

    private void Attack()
    {

        if (Input.GetMouseButtonDown(0) && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            currentAttack++;
            isAttacking = true;

            if (currentAttack > 3)
                currentAttack = 1;

            //Reset
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            //Call Attack Triggers
            playerAnim.SetTrigger("Attack" + currentAttack);

            //Reset Timer
            timeSinceAttack = 0;
        }
    }
    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.C))
            if (thirdPersonController.MoveSpeed != 0) StartCoroutine(Rolling());
    }
    private IEnumerator Rolling()
    {
        // ȸ�� �ִϸ��̼� Ʈ���� ����
        playerAnim.SetTrigger("Dodge");

        // ȸ�� ���� ����
        isDodgeing = true;

        // ĳ���� ��Ʈ�ѷ��� ���� ���̿� ���� ����
        float originalHeight = characterController.height;
        Vector3 originalCenter = characterController.center;

        // ��ǥ ���̿� ���� ���
        float targetHeight = originalHeight / 2;
        Vector3 targetCenter = new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z);

        // �ִϸ��̼� Ŀ���� ���� �ð� ���
        float duration = dodgeCurve.keys[dodgeCurve.length - 1].time;
        float time = 0;

        // dodgeCurve�� ����Ͽ� �ε巴�� ���̿� ���� ����
        while (time < duration)
        {
            // �ִϸ��̼� Ŀ�� �� ��
            float curveValue = dodgeCurve.Evaluate(time);

            // ���� �ð��� ���� ���̿� ���͸� ���� ����
            characterController.height = Mathf.Lerp(originalHeight, targetHeight, curveValue);
            characterController.center = Vector3.Lerp(originalCenter, targetCenter, curveValue);

            // �ð� ������Ʈ
            time += Time.deltaTime;
            yield return null;
        }
        characterController.height = originalHeight;
        characterController.center = originalCenter;

        // ȸ�� ���� ����
        isDodgeing = false;
    }


    public void ResetAttack()
    {
        isAttacking = false;
    }
}
