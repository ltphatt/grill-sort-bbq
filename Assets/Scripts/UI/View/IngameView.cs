using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameView : BaseView
{
    public override void Show()
    {
        base.Show();
        Time.timeScale = 1f;
    }

    public void OnHomeButtonClicked()
    {
        ViewController.Instance.ShowView(ViewType.Home, false);
    }

    public void OnPauseButtonClicked()
    {
        PopupController.Instance.Show(PopupType.Pause);
    }
}
