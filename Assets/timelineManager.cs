using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class timelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector levelOverviewTimeline;
    // Start is called before the first frame update
    void Start()
    {
        levelOverviewTimeline.time = 0;
        levelOverviewTimeline.Stop();
        levelOverviewTimeline.Evaluate();
    }
}
