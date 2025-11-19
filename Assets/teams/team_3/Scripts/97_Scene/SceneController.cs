// using MyPooler;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonObject<SceneController>
{
    public SceneName currentScene { get; private set; }

    void Start()
    {
        currentScene = Util.ParseEnumFromString<SceneName>(SceneManager.GetActiveScene().name);
    }

    public void ChangeScene(SceneName sceneName)
    {
        RunChangeSceneProcess(sceneName);
        SceneManager.LoadScene(sceneName.ToString());
    }

    public IEnumerator ChangeSceneWithLoading(SceneName targetScene)
    {
        yield return StartCoroutine(FadeManager.Instance.FadeOut());
        switch (targetScene)
        {
            case SceneName.Loading:
                // 주의: 실제로 Loading Scene은 위 함수를 통해 불러올 일이 없어야함
                Debug.LogError("Loading Scene cannot be loaded directly.");
                break;
            default:
                currentScene = targetScene; // Loading Scene을 부르고 나서 실제 우리가 원하는 TargetScene을 켜야하기 때문에 설정(더 좋은 방식이 있다면 개선 가능)
                // ObjectPooler.Instance.ResetAllPools();
                ChangeScene(SceneName.Loading);
                break;
        }
        yield return null;
    }

    public void RunChangeSceneProcess(SceneName sceneName)
    {
    }

    public void LoadTitle()
    {
        // StartCoroutine(ChangeSceneWithLoading(SceneName.Title));
    }

    public bool GetIsBossStage(SceneName sceneName)
    {
        switch (sceneName)
        {
            default:
                return false;
        }
    }
}