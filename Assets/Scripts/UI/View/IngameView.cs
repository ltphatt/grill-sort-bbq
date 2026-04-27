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
        ViewController.Ins.ShowView(ViewType.Home, false);
    }
}
