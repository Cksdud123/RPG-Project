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

    //장비 파라미터
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;
    public bool isEquipped;

    //막기
    public bool isBlocking;

    //발차기
    public bool isKicking;

    //공격
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;

    // 닷지
    [SerializeField] AnimationCurve dodgeCurve;
    //회피
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
        // 회피 애니메이션 트리거 설정
        playerAnim.SetTrigger("Dodge");

        // 회피 상태 설정
        isDodgeing = true;

        // 캐릭터 컨트롤러의 원래 높이와 센터 저장
        float originalHeight = characterController.height;
        Vector3 originalCenter = characterController.center;

        // 목표 높이와 센터 계산
        float targetHeight = originalHeight / 2;
        Vector3 targetCenter = new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z);

        // 애니메이션 커브의 지속 시간 계산
        float duration = dodgeCurve.keys[dodgeCurve.length - 1].time;
        float time = 0;

        // dodgeCurve를 사용하여 부드럽게 높이와 센터 변경
        while (time < duration)
        {
            // 애니메이션 커브 값 평가
            float curveValue = dodgeCurve.Evaluate(time);

            // 현재 시간에 따라 높이와 센터를 선형 보간
            characterController.height = Mathf.Lerp(originalHeight, targetHeight, curveValue);
            characterController.center = Vector3.Lerp(originalCenter, targetCenter, curveValue);

            // 시간 업데이트
            time += Time.deltaTime;
            yield return null;
        }
        characterController.height = originalHeight;
        characterController.center = originalCenter;

        // 회피 상태 해제
        isDodgeing = false;
    }


    public void ResetAttack()
    {
        isAttacking = false;
    }
}
