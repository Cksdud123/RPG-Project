using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    //Third Person Controller References
    [SerializeField]
    private Animator playerAnim;
    private ThirdPersonController thirdPersonController;
    private CharacterController characterController;


    //장비 파라미터
    [Header("Equipment Param")]
    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;
    public bool isEquipped;

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
    public bool isDamaged;

    float dodgeTimer, dodge_coolDown;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip EquipmentSword;

    [Header("Attack Sound")]
    public AudioClip Attack1Sound;
    public AudioClip Attack2Sound;
    public AudioClip Attack3Sound;

    [Header("Pain Sound")]
    public AudioClip painSound;

    [Header("InventoryisAcrive?")]
    [SerializeField] private GameObject PanelInventory;
    [SerializeField] private GameObject equipment;
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

    public void Kick()
    {
        if (Input.GetKeyDown(KeyCode.G) && playerAnim.GetBool("Grounded")) playerAnim.SetTrigger("Kick");
    }

    private void Attack()
    {
        if (PanelInventory.activeInHierarchy || equipment.activeInHierarchy) return;

        if (Input.GetMouseButtonDown(0) && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.7f)
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
            // 애니메이션 커브 값 저장
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

    void SwordSound()
    {
        audioSource.clip = EquipmentSword;
        audioSource.Play();
    }
    void SwordAttack1()
    {
        audioSource.clip = Attack1Sound;
        audioSource.Play();
    }
    void SwordAttack2()
    {
        audioSource.clip = Attack2Sound;
        audioSource.Play();
    }
    void SwordAttack3()
    {
        audioSource.clip = Attack3Sound;
        audioSource.Play();
    }
    void PainSound()
    {
        audioSource.clip = painSound;
        audioSource.volume = 0.3f;
        audioSource.Play();
        audioSource.volume = 0.7f;
    }
    public void ResetAttack()
    {
        isAttacking = false;
    }

    void OffPlayerMove()
    {
        isDamaged = true;
    }
    void OnPlayerMove()
    {
        isDamaged = false;
    }
}
