using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OvrAvatarHand : MonoBehaviour, IAvatarPart
{
    public Hand handScript;
    float alpha = 1.0f;
    Rigidbody rigidBody = null;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void UpdatePose(OvrAvatarDriver.ControllerPose pose)
    {
        if (rigidBody != null)
        {
            rigidBody.detectCollisions = handScript.mHandState == Hand.State.EMPTY && pose.handTrigger >= 0.75f;
        }
    }

    public void SetAlpha(float alpha)
    {
        this.alpha = alpha;
    }

    public void OnAssetsLoaded()
    {
        SetAlpha(this.alpha);
    }
}