using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OvrAvatarHand : MonoBehaviour
{
    public OvrAvatarDriver AvatarDriver;
    public Hand handScript;
    float alpha = 1.0f;
    Rigidbody rigidBody = null;
    OVRInput.Controller controller;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (name.ToLower().Contains("left"))
        {
            controller = OVRInput.Controller.LTouch;
        }
        else
        {
            controller = OVRInput.Controller.RTouch;
        }
    }

    public void Update()
    {
        if (rigidBody != null)
        {
            rigidBody.detectCollisions = handScript.mHandState == Hand.State.EMPTY && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) >= 0.75f;
        }
    }
}