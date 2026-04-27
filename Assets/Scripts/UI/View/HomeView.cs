using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeView : BaseView
{
    public override void Show()
    {
        base.Show();
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        ViewController.Ins.ShowView(ViewType.Ingame);
        GameController.Instance.ToNextLevel();
    }

    public void OnHomeSettingsButtonClicked()
    {
        PopupController.Ins.Show(PopupType.Settings);
    }
}
