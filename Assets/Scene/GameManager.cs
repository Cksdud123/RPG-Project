using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject player;

    private GameObject LoadPosition;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("BossMap");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("Playground");
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� �ε� �� �÷��̾� ��ġ ���� �ڷ�ƾ ����
        StartCoroutine(SetPlayerPosition());
    }

    IEnumerator SetPlayerPosition()
    {
        // �� ������ ���
        yield return null;

        // �÷��̾�� LoadPosition ������Ʈ ã��
        player = GameObject.FindWithTag("Player");
        LoadPosition = GameObject.FindWithTag("Position");
        if (player != null && LoadPosition != null) player.transform.position = LoadPosition.transform.position;
    }
}
