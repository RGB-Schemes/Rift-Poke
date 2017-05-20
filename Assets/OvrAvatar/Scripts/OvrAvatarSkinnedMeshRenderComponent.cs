using UnityEngine;
using System.Collections;
using System;

public class OvrAvatarSkinnedMeshRenderComponent : OvrAvatarRenderComponent
{
    private struct FingerBone
    {
        public readonly float Radius;
        public readonly float Height;

        public FingerBone(float radius, float height)
        {
            Radius = radius;
            Height = height;
        }

        public Vector3 GetCenter(bool isLeftHand)
        {
            return new Vector3(((isLeftHand) ? -1 : 1) * Height / 2.0f, 0, 0);
        }
    };

    private readonly FingerBone Phalanges = new FingerBone(0.01f, 0.03f);
    private readonly FingerBone Metacarpals = new FingerBone(0.01f, 0.05f);

    SkinnedMeshRenderer mesh;
    Transform[] bones;

    private void CreateCollider(Transform transform)
    {
        if (!transform.gameObject.GetComponent(typeof(CapsuleCollider)) && 
            !transform.gameObject.GetComponent(typeof(SphereCollider)) &&
            transform.name.Contains("hands"))
        {
            if (transform.name.Contains("thumb") ||
                transform.name.Contains("index") ||
                transform.name.Contains("middle") ||
                transform.name.Contains("ring") ||
                transform.name.Contains("pinky"))
            {
                if (!transform.name.EndsWith("0"))
                {
                    CapsuleCollider collider = transform.gameObject.AddComponent<CapsuleCollider>();
                    if (!transform.name.EndsWith("1"))
                    {
                        collider.radius = Phalanges.Radius;
                        collider.height = Phalanges.Height;
                        collider.center = Phalanges.GetCenter(transform.name.Contains("_l_"));
                        collider.direction = 0;
                    }
                    else
                    {
                        collider.radius = Metacarpals.Radius;
                        collider.height = Metacarpals.Height;
                        collider.center = Metacarpals.GetCenter(transform.name.Contains("_l_"));
                        collider.direction = 0;
                    }
                }
            }
            else if (transform.name.Contains("grip"))
            {
                SphereCollider collider = transform.gameObject.AddComponent<SphereCollider>();
                collider.radius = 0.04f;
                collider.center = new Vector3(
                    ((transform.name.Contains("_l_")) ? -1 : 1) * 0.01f,
                    0.01f, 0.02f);
            }
        }
    }

    internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRender skinnedMeshRender, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
    {
        mesh = CreateSkinnedMesh(skinnedMeshRender.meshAssetID, skinnedMeshRender.visibilityMask, false, thirdPersonLayer, firstPersonLayer, sortOrder);
        bones = mesh.bones;

        foreach(Transform bone in bones)
        {
            if (!bone.name.Contains("ignore"))
            {
                CreateCollider(bone);
            }
        }
    }

    internal void UpdateSkinnedMeshRender(OvrAvatar avatar, ovrAvatarRenderPart_SkinnedMeshRender meshRender)
    {
        UpdateSkinnedMesh(avatar, mesh, bones, meshRender.localTransform, meshRender.visibilityMask, meshRender.skinnedPose);
        UpdateAvatarMaterial(mesh.sharedMaterial, meshRender.materialState);
    }
}
