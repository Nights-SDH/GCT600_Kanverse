using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public CanvasGroup fadeCanvasGroup;
    public GameObject fadePanel;
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //StartCoroutine(FadeIn()); 
    }

    public IEnumerator FadeIn()
    {
        fadePanel.SetActive(true);
        fadeCanvasGroup.alpha = 1;
        yield return new WaitForSecondsRealtime(1f);

        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.unscaledDeltaTime / fadeInDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0;
        fadePanel.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        fadePanel.SetActive(true);
        fadeCanvasGroup.alpha = 0;

        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += Time.unscaledDeltaTime / fadeOutDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
    }

    public IEnumerator StartFadeInWithSceneTitle()
    {
        yield return null;
        StartCoroutine(FadeIn());
        // SoundManager.Instance.PlaySceneBGM();
    }

    public void FadeOutWithSceneTitle()
    {
        StartCoroutine(StartFadeInWithSceneTitle());
    }
}