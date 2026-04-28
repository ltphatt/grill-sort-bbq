using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class GrillEffect : MonoBehaviour
{
    [SerializeField] SkeletonGraphic smokeAnim;
    float smokeInterval = 10f;
    float timer = 0f;

    void Start()
    {
        smokeInterval = Random.Range(15f, 30f);
    }

    void OnEnable()
    {
        smokeAnim.AnimationState.Complete += OnSmokeAnimComplete;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= smokeInterval)
        {
            smokeAnim.gameObject.SetActive(true);
            smokeAnim.AnimationState.SetAnimation(0, "FINAL", false);

            smokeInterval = Random.Range(15f, 30f);
            timer = 0f;
        }
    }

    void OnSmokeAnimComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "FINAL")
        {
            smokeAnim.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        smokeAnim.AnimationState.Complete -= OnSmokeAnimComplete;
    }
}
