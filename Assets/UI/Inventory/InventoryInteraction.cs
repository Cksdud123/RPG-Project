using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    public GameObject PanelInventory;

    [Header("Ray")]
    [SerializeField] private int hitrange = 10;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask ShopLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject PlayerCamera;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;
    [SerializeField] TextMeshProUGUI txt_item;
    [SerializeField] private GameObject CrossHair;
    [SerializeField] private GameObject DropEventPanel;
    [SerializeField] private GameObject equipment;
    [SerializeField] private GameObject ShopPanel;

    private bool isEquipmentActive = false;

    // Update is called once per frame
    void Update()
    {
        ActiveInventory();
        ActiveEquipment();
        UpdateTimeScale();
        CheckItem();
    }
    public void ActiveInventory()
    {
        if (PanelInventory.activeInHierarchy && Input.GetKeyDown(KeyCode.I))
        {
            Cursor.lockState = CursorLockMode.Locked;
            DropEventPanel.SetActive(false);
            PanelInventory.SetActive(false);

            CrossHair.SetActive(true);
            PlayerCamera.SetActive(true);
            txt_item.gameObject.SetActive(true);
        }
        else if (!PanelInventory.activeInHierarchy && Input.GetKeyDown(KeyCode.I))
        {
            Cursor.lockState = CursorLockMode.None;
            PanelInventory.transform.position = new Vector3(Camera.main.pixelWidth / 2 + 500.0f, (Camera.main.pixelHeight / 2));
            DropEventPanel.SetActive(true);
            PanelInventory.SetActive(true);
            PlayerCamera.SetActive(false);

            CrossHair.SetActive(false);
            txt_item.gameObject.SetActive(false);
        }
    }
    public void ActiveEquipment()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEquipmentActive = !isEquipmentActive;

            if (isEquipmentActive)
            {
                Cursor.lockState = CursorLockMode.None;
                PlayerCamera.SetActive(false);
                equipment.transform.position = new Vector3(Camera.main.pixelWidth / 2 - 500.0f, (Camera.main.pixelHeight / 2));
                equipment.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                PlayerCamera.SetActive(true);
                equipment.SetActive(false);
            }
        }
    }
    // �����ϳ��� ���������� ����
    private void UpdateTimeScale()
    {
        if (PanelInventory.activeInHierarchy || isEquipmentActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
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
                txt_item.text = $"������ �ݱ�(F)";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    inventory.pickUpItem(hit.collider.GetComponent<ItemInfo>());
                }
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, ShopLayer))
        {
            if (!hit.collider.GetComponent<Collider>()) return;

            else
            {
                txt_item.text = $"�����̿�(F)";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    // �Լ�
                    ActiveShop();
                }
            }
        }
        else
        {
            txt_item.text = string.Empty;
        }
    }
    private void ActiveShop()
    {
        Cursor.lockState = CursorLockMode.None;
        // ����, �κ��丮�г� Ȱ��ȭ
        ShopPanel.gameObject.SetActive(true);
        PanelInventory.gameObject.SetActive(true);

        CrossHair.SetActive(false);
        txt_item.gameObject.SetActive(false);

        // ���� �����г��� ���� �κ��丮�� ���� �������� IŰ�� ������ �κ��丮�� ���� IŰ�� ������ �����гΰ� �κ��丮�г��� ���ÿ� �������� ������
    }
    private void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * hitrange);
    }
}
