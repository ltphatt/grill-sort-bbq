using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeView : BaseView
{
    public void StartGame()
    {
        ViewController.Ins.ShowView(ViewType.Ingame);
        LevelManager.Instance.ToNextLevel();
    }
}
