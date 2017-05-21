using UnityEngine;
using System.Collections;
using System;
using Oculus.Avatar;

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

    Shader surface;
    Shader surfaceSelfOccluding;
    bool previouslyActive = false;

    private readonly FingerBone Phalanges = new FingerBone(0.01f, 0.03f);
    private readonly FingerBone Metacarpals = new FingerBone(0.01f, 0.05f);

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

    internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRender skinnedMeshRender, Shader surface, Shader surfaceSelfOccluding, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
    {
        this.surfaceSelfOccluding = surfaceSelfOccluding != null ? surfaceSelfOccluding :  Shader.Find("OvrAvatar/AvatarSurfaceShaderSelfOccluding");
        this.surface = surface != null ? surface : Shader.Find("OvrAvatar/AvatarSurfaceShader");
        this.mesh = CreateSkinnedMesh(skinnedMeshRender.meshAssetID, skinnedMeshRender.visibilityMask, thirdPersonLayer, firstPersonLayer, sortOrder);
        bones = mesh.bones;
        UpdateMeshMaterial(skinnedMeshRender.visibilityMask, mesh);
        foreach (Transform bone in bones)
        {
            if (!bone.name.Contains("ignore"))
            {
                CreateCollider(bone);
            }
        }
    }

    public void UpdateSkinnedMeshRender(OvrAvatarComponent component, OvrAvatar avatar, IntPtr renderPart)
    {
        ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRender_GetVisibilityMask(renderPart);
        ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRender_GetTransform(renderPart);
        UpdateSkinnedMesh(avatar, bones, localTransform, visibilityMask, renderPart);

        UpdateMeshMaterial(visibilityMask, mesh == null ? component.RootMeshComponent : mesh);
        bool isActive = this.gameObject.activeSelf;

        if( mesh != null )
        {
            bool changedMaterial = CAPI.ovrAvatarSkinnedMeshRender_MaterialStateChanged(renderPart);
            if (changedMaterial || (!previouslyActive && isActive))
            {
                ovrAvatarMaterialState materialState = CAPI.ovrAvatarSkinnedMeshRender_GetMaterialState(renderPart);
                component.UpdateAvatarMaterial(mesh.sharedMaterial, materialState);
            }
        }
        previouslyActive = isActive;
    }

    private void UpdateMeshMaterial(ovrAvatarVisibilityFlags visibilityMask, SkinnedMeshRenderer rootMesh)
    {
        Shader shader = (visibilityMask & ovrAvatarVisibilityFlags.SelfOccluding) != 0 ? surfaceSelfOccluding : surface;
        if (rootMesh.sharedMaterial == null || rootMesh.sharedMaterial.shader != shader)
        {
            rootMesh.sharedMaterial = CreateAvatarMaterial(gameObject.name + "_material", shader);
        }
    }
}
