using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameView : BaseView
{
    public void OnHomeButtonClicked()
    {
        ViewController.Ins.ShowView(ViewType.Home, false);
    }
}
