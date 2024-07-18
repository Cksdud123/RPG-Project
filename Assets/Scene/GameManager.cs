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
        // 씬 로드 후 플레이어 위치 설정 코루틴 실행
        StartCoroutine(SetPlayerPosition());
    }

    IEnumerator SetPlayerPosition()
    {
        // 한 프레임 대기
        yield return null;

        // 플레이어와 LoadPosition 오브젝트 찾기
        player = GameObject.FindWithTag("Player");
        LoadPosition = GameObject.FindWithTag("Position");
        if (player != null && LoadPosition != null) player.transform.position = LoadPosition.transform.position;
    }
}
