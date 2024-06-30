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
    [SerializeField] private LayerMask QuestNPCLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject PlayerCamera;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;
    [SerializeField] TextMeshProUGUI txt_item;
    [SerializeField] private GameObject CrossHair;
    [SerializeField] private GameObject DropEventPanel;
    [SerializeField] private GameObject equipment;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject ForgePanel;
    [SerializeField] private GameObject DialougePanel;
    [SerializeField] private GameObject HotBarPanel;
    [SerializeField] private GameObject LevelBarPanel;
    [SerializeField] private GameObject MinimapPanel;

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
            MinimapPanel.SetActive(true);
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
            MinimapPanel.SetActive(false);
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
        if (PanelInventory.activeInHierarchy || isEquipmentActive || DialougePanel.activeInHierarchy) Time.timeScale = 0f;
        else Time.timeScale = 1.0f;
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
                if (Input.GetKeyDown(KeyCode.F)) inventory.pickUpItem(hit.collider.GetComponent<ItemInfo>(), true);
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, ShopLayer))
        {
            if (!hit.collider.GetComponent<Collider>()) return;
            else
            {
                txt_item.text = $"상점이용(I)";
                if (Input.GetKeyDown(KeyCode.I)) ActiveShop();
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, ForgeLayer))
        {
            if (!hit.collider.GetComponent<Collider>()) return;
            else
            {
                txt_item.text = $"강화하기(I)";
                if (Input.GetKeyDown(KeyCode.I)) ActiveForge();
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, QuestNPCLayer))
        {
            if (!hit.collider.GetComponent<Collider>()) return;
            else
            {
                txt_item.text = $"대화하기(Q)";
                if (Input.GetKeyDown(KeyCode.Q)) ActiveDialouge();
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
            // 상점, 인벤토리패널 활성화
            ShopPanel.SetActive(true);
            PanelInventory.SetActive(true);

            DeactivePlayerUI();
        }
        else
        {
            // 상점, 인벤토리패널 비활성화
            ShopPanel.SetActive(false);
            PanelInventory.SetActive(false);

            ActivePlayerUI();
        }
    }
    private void ActiveForge()
    {
        if (!ForgePanel.activeInHierarchy)
        {
            // 상점, 인벤토리패널 활성화
            ForgePanel.SetActive(true);
            PanelInventory.SetActive(true);

            DeactivePlayerUI();
        }
        else
        {
            // 상점, 인벤토리패널 비활성화
            ForgePanel.SetActive(false);
            PanelInventory.SetActive(false);

            ActivePlayerUI();
        }
    }
    private void ActiveDialouge()
    {
        if (!DialougePanel.activeInHierarchy)
        {
            DialougePanel.SetActive(true);

            HotBarPanel.SetActive(false);
            LevelBarPanel.SetActive(false);
            MinimapPanel.SetActive(false);

            DeactivePlayerUI();
        }
        else
        {
            DialougePanel.SetActive(false);

            HotBarPanel.SetActive(true);
            LevelBarPanel.SetActive(true);
            MinimapPanel.SetActive(true);

            ActivePlayerUI();
        }
    }
    private void ActivePlayerUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        DropEventPanel.SetActive(true);
        CrossHair.SetActive(true);
        txt_item.gameObject.SetActive(true);
        PlayerCamera.SetActive(true);
    }
    private void DeactivePlayerUI()
    {
        Cursor.lockState = CursorLockMode.None;
        DropEventPanel.SetActive(false);
        CrossHair.SetActive(false);
        txt_item.gameObject.SetActive(false);
        PlayerCamera.SetActive(false);
    }
}