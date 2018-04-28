using UnityEngine;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;

public class SimplePlatformManager : MonoBehaviour {
    public OvrAvatar Avatar;
    void Awake()
    {
        Oculus.Platform.Core.Initialize();
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(UserLoggedInCallback);
        Oculus.Platform.Request.RunCallbacks();
    }

    private void UserLoggedInCallback(Message<User> message)
    {
        if (!message.IsError)
        {
            Avatar.oculusUserID = message.Data.ID;
        }
    }
}
