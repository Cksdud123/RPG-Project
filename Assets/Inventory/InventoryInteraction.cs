using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    public GameObject PanelInventory;

    [SerializeField] private int hitrange = 10;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private Camera cam;

    [SerializeField] TextMeshProUGUI txt_item;

    // Update is called once per frame
    void Update()
    {
        ActiveInventory();
        CheckItem();
    }
    public void ActiveInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (PanelInventory.activeSelf)
            {
                // �κ��丮�� Ȱ��ȭ ������ ��, ��Ȱ��ȭ�մϴ�.
                Cursor.lockState = CursorLockMode.Locked;
                PanelInventory.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                // �κ��丮�� ��Ȱ��ȭ ������ ��, Ȱ��ȭ�մϴ�.
                Cursor.lockState = CursorLockMode.None;
                PanelInventory.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
                PanelInventory.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void CheckItem()
    {
        RaycastHit hit;

        // ī�޶󿡼� ������ ���̸� �߻��մϴ�.
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, itemLayer))
        {

            // ������ ���̾ �ƴϸ� ����
            if (!hit.collider.GetComponent<ItemInfo>()) return;

            else
            {
                txt_item.text = $"PRESS 'F' GET ITEM {hit.collider.GetComponent<ItemInfo>().iteminfo.name} AMOUNT : {hit.collider.GetComponent<ItemInfo>().amount}";

                if (Input.GetKeyDown(KeyCode.F))
                {

                }
            }
        }
        else
        {
            txt_item.text = string.Empty;
        }
    }
    private void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * hitrange);
    }
}
