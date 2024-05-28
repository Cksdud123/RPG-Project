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
                // 인벤토리가 활성화 상태일 때, 비활성화합니다.
                Cursor.lockState = CursorLockMode.Locked;
                PanelInventory.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                // 인벤토리가 비활성화 상태일 때, 활성화합니다.
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

        // 카메라에서 앞으로 레이를 발사합니다.
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hitrange, itemLayer))
        {

            // 아이템 레이어가 아니면 리턴
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
