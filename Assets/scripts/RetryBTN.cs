using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryBTN : MonoBehaviour
{
    public GameObject CreaterBan;
    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void EXIT()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void CreaterBanner()
    {
        CreaterBan.SetActive(true);
    }
    public void StageScene()
    {
       
        SceneManager.LoadScene("StageScene");

    }
    public void StageEasyScene()
    {
        DifficultSetting.Instance.Setting(DifficultSetting.Difficulty.Easy);
        Debug.Log(DifficultSetting.Instance.Getting());
        Retry();

    }
    public void StageNormalScene()
    {
        DifficultSetting.Instance.Setting(DifficultSetting.Difficulty.Normal);
        Debug.Log(DifficultSetting.Instance.Getting());
        Retry();
    }
    public void StageHardScene()
    {
        DifficultSetting.Instance.Setting(DifficultSetting.Difficulty.Hard);
        Debug.Log(DifficultSetting.Instance.Getting());
        Retry();
    }
}
