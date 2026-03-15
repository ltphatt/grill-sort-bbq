using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] string mainSceneName = "Main";

    public void StartGame()
    {
        SceneManager.LoadScene(mainSceneName);
        AudioManager.Instance.StopBGM();
    }

}
