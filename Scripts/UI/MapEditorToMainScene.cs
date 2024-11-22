using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapEditorToMainScene : MonoBehaviour
{
    public string sceneToLoad;
    public Slider progressBar;
    public Text progressText;

    //void Start()
    //{
    //    StartCoroutine(LoadSceneAsync());
    //}

    private void OnEnable()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";
            yield return null;
        }
    }
}
