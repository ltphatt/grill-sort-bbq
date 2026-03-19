using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] float delay = 3f;
    [SerializeField] string homeSceneName = "Home";

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    // Pseudo loading screen
    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this.homeSceneName);

        operation.allowSceneActivation = false;

        float elapsedTime = 0f;
        while (elapsedTime < delay || operation.progress < .9f)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / delay);

            loadingBar.value = progress;
            loadingText.text = $"{Mathf.RoundToInt(progress * 100)}%";

            yield return null;
        }

        loadingBar.value = 1f;
        loadingText.text = "100%";

        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
        AudioManager.Instance.PlayBGM("Casual", 0.75f);
    }
}
