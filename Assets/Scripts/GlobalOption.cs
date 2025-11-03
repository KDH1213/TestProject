using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum GameModeType
{
    None = 0,
    GamePlay,
    GamePause,
    GameStop,

    End
}
public class GlobalOption : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate;

    private GameModeType currentMode = GameModeType.GamePlay;

    public static float prevTimeScale;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        prevTimeScale = Time.timeScale;

        //#if UNITY_EDITOR
        //        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        //        UnityEngine.Rendering.DebugManager.instance.displayRuntimeUI = false;
        //#endif
    }

    public void OnPause()
    {
        if (currentMode == GameModeType.GamePause)
        {
            Time.timeScale = 0f;
            return;
        }

        currentMode= GameModeType.GamePause;
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnResume()
    {
        currentMode = GameModeType.GamePlay;
        Time.timeScale = prevTimeScale;
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
