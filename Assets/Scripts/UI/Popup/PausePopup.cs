using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : BasePopup
{
    protected override void OnShow()
    {
        Time.timeScale = 0f;
    }

    protected override void OnHide()
    {
        Time.timeScale = 1f;
    }

    public void OnResumeButtonClicked()
    {
        Hide();
    }

    public void OnRestartButtonClicked()
    {
        Hide();
        ViewController.Instance.ShowView(ViewType.Ingame, false);
        GameController.Instance.RestartLevel();
    }

    public void OnHomeButtonClicked()
    {
        Hide();
        ViewController.Instance.ShowView(ViewType.Home, false);
    }
}
