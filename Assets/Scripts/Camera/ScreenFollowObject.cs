using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ScreenFollowObject : MonoBehaviour
{
    private void Awake() {
        ScreenFollowBehaviour follower = Camera.main.GetComponent<ScreenFollowBehaviour>();
        if (follower != null) {
            follower.AddObject(transform);
        }
    }
}
