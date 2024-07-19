using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    [SerializeField] Image progressBar;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneProcess()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    op.completed += OnSceneLoaded;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(AsyncOperation op)
    {
        if (this != null)
        {
            StartCoroutine(WaitAndMovePlayer());
        }
    }

    IEnumerator WaitAndMovePlayer()
    {
        yield return null;

        MovePlayerToLoadPosition();
        Destroy(gameObject);
    }

    private void MovePlayerToLoadPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject loadPosition = GameObject.FindWithTag("Position");

        if (player != null && loadPosition != null)
        {
            player.transform.position = loadPosition.transform.position;
        }
    }
}
