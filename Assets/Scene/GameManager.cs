using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static List<string> dontDestroyObjects = new List<string>();
    private void Awake()
    {
        if (dontDestroyObjects.Contains(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        dontDestroyObjects.Add(gameObject.name);
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadingSceneController.LoadScene("BossMap");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadingSceneController.LoadScene("Playground");
        }
    }
}
