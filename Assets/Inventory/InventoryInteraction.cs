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
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject PlayerCamera;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;
    [SerializeField] TextMeshProUGUI txt_item;
    [SerializeField] private GameObject CrossHair;
    

    // Update is called once per frame
    void Update()
    {
        ActiveInventory();
        CheckItem();
    }
    public void ActiveInventory()
    {
        if (PanelInventory.activeInHierarchy && Input.GetKeyDown(KeyCode.I))
        {
            Cursor.lockState = CursorLockMode.Locked;
            PanelInventory.SetActive(false);
            CrossHair.SetActive(true);
            PlayerCamera.SetActive(true);
            txt_item.gameObject.SetActive(true);
            Time.timeScale = 1f;
        }
        else if (!PanelInventory.activeInHierarchy && Input.GetKeyDown(KeyCode.I))
        {
            Cursor.lockState = CursorLockMode.None;
            PanelInventory.transform.position = new Vector3(Camera.main.pixelWidth / 2 - 100.0f, (Camera.main.pixelHeight / 2));
            PanelInventory.SetActive(true);
            CrossHair.SetActive(false);
            PlayerCamera.SetActive(false);
            txt_item.gameObject.SetActive(false);
            Time.timeScale = 0f;
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
