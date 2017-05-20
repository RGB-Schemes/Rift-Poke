// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Oculus.Platform.Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class UserAndRoom
  {
    public readonly Room Room;
    public readonly User User;


    public UserAndRoom(IntPtr o)
    {
      Room = new Room(CAPI.ovr_UserAndRoom_GetRoom(o));
      User = new User(CAPI.ovr_UserAndRoom_GetUser(o));
    }
  }

  public class UserAndRoomList : DeserializableList<UserAndRoom> {
    public UserAndRoomList(IntPtr a) {
      var count = (int)CAPI.ovr_UserAndRoomArray_GetSize(a);
      _Data = new List<UserAndRoom>(count);
      for (int i = 0; i < count; i++) {
        _Data.Add(new UserAndRoom(CAPI.ovr_UserAndRoomArray_GetElement(a, (UIntPtr)i)));
      }

      _NextUrl = CAPI.ovr_UserAndRoomArray_GetNextUrl(a);
    }

  }
}
