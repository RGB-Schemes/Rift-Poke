// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Oculus.Platform.Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class MatchmakingEnqueuedUser
  {
    public readonly Dictionary<string, string> CustomData;
    public readonly User User;


    public MatchmakingEnqueuedUser(IntPtr o)
    {
      CustomData = CAPI.DataStoreFromNative(CAPI.ovr_MatchmakingEnqueuedUser_GetCustomData(o));
      User = new User(CAPI.ovr_MatchmakingEnqueuedUser_GetUser(o));
    }
  }

  public class MatchmakingEnqueuedUserList : DeserializableList<MatchmakingEnqueuedUser> {
    public MatchmakingEnqueuedUserList(IntPtr a) {
      var count = (int)CAPI.ovr_MatchmakingEnqueuedUserArray_GetSize(a);
      _Data = new List<MatchmakingEnqueuedUser>(count);
      for (int i = 0; i < count; i++) {
        _Data.Add(new MatchmakingEnqueuedUser(CAPI.ovr_MatchmakingEnqueuedUserArray_GetElement(a, (UIntPtr)i)));
      }

    }

  }
}
