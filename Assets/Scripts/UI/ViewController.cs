using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public static ViewController Ins;
    [SerializeField] private List<BaseView> views;
    [SerializeField] ViewType startView = ViewType.Home;
    Dictionary<ViewType, BaseView> viewDictionary = new();
    Stack<BaseView> viewHistory = new();
    BaseView currentView;

    void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
            return;
        }
        Ins = this;
        InitViews();
    }

    void Start()
    {
        ShowView(startView, false);
    }

    void InitViews()
    {
        viewDictionary.Clear();
        foreach (var view in views)
        {
            view.Initialize();
            view.Hide();

            if (!viewDictionary.ContainsKey(view.viewType))
            {
                viewDictionary.Add(view.viewType, view);
            }
        }
    }

    public void ShowView(ViewType viewType, bool saveHistory = true)
    {
        if (!viewDictionary.ContainsKey(viewType))
        {
            Debug.LogError($"View of type {viewType} not found!");
            return;
        }

        BaseView nextView = viewDictionary[viewType];
        if (currentView == nextView) return;

        if (currentView != null)
        {
            if (saveHistory)
            {
                viewHistory.Push(currentView);
            }
            currentView.Hide();
        }
        nextView.Show();
        currentView = nextView;
        Debug.Log($"Show view: {viewType}, save history: {saveHistory}");
    }

    public void Back()
    {
        if (viewHistory.Count > 0)
        {
            BaseView previousView = viewHistory.Pop();
            if (currentView != null)
            {
                currentView.Hide();
            }
            previousView.Show();
            currentView = previousView;
        }
    }
}
