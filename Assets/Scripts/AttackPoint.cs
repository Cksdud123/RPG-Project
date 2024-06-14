using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            int RandomHit = Random.Range(1, 3);

            Enemy enemy = hits[0].gameObject.GetComponentInParent<Enemy>();
            EpicEnemyHealthBar epicEnemy = hits[0].gameObject.GetComponentInParent<EpicEnemyHealthBar>();
            PlayerStatus player = hits[0].gameObject.GetComponentInParent<PlayerStatus>();
            if (enemy != null)
            {
                enemy.Damage(damage);
                gameObject.SetActive(false);
            }
            else if (epicEnemy != null)
            {
                epicEnemy.Damage(damage);
                gameObject.SetActive(false);
            }
            else if (player != null)
            {
                player.PlayerDamage(damage);
                player.animator.SetTrigger("Hit" + RandomHit);
                StartCoroutine(DisableCharacterControllerTemporarily(player, Random.Range(0.3f, 0.7f)));
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator DisableCharacterControllerTemporarily(PlayerStatus player, float duration)
    {
        // 공격에 맞으면 잠시동안 이동과 회전을 제한
        ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();

        playerController.enabled = false;
        yield return new WaitForSeconds(duration);
        playerController.enabled = true;
    }
}
