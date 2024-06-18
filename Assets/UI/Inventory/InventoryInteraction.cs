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
    [SerializeField] private LayerMask ForgeLayer;
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
            PanelInventory.transform.position = new Vector3(Camera.main.pixelWidth / 2 + 600.0f, (Camera.main.pixelHeight / 2));
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
    // 둘중하나라도 열려있으면 정지
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

        // 카메라에서 앞으로 레이를 발사합니다.
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, itemLayer))
        {

            // 아이템 레이어가 아니면 리턴
            if (!hit.collider.GetComponent<ItemInfo>()) return;

            else
            {
                txt_item.text = $"아이템 줍기(F)";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    inventory.pickUpItem(hit.collider.GetComponent<ItemInfo>(), true);
                }
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, ShopLayer))
        {
            if (!hit.collider.GetComponent<Collider>()) return;

            else
            {
                txt_item.text = $"상점이용(I)";
                if (Input.GetKeyDown(KeyCode.I))
                {
                    ActiveShop();
                }
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, ForgeLayer))
        {
            if (!hit.collider.GetComponent<Collider>()) return;

            else
            {
                txt_item.text = $"강화하기(I)";
                if (Input.GetKeyDown(KeyCode.I))
                {
                    //
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
        if (!ShopPanel.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            // 상점, 인벤토리패널 활성화
            ShopPanel.SetActive(true);
            PanelInventory.SetActive(true);

            DropEventPanel.SetActive(false);
            CrossHair.SetActive(false);
            txt_item.gameObject.SetActive(false);
            PlayerCamera.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            // 상점, 인벤토리패널 비활성화
            ShopPanel.SetActive(false);
            PanelInventory.SetActive(false);

            DropEventPanel.SetActive(true);
            CrossHair.SetActive(true);
            txt_item.gameObject.SetActive(true);
            PlayerCamera.SetActive(true);
        }
    }
    private void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * hitrange);
    }
}
