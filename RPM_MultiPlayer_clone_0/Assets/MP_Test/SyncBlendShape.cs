using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SyncBlendShape : NetworkBehaviour
{
    private SkinnedMeshRenderer player_skinnedMeshRenderer;
    public int blendShapeCount = 67;
    private float[] blendShapeValue = null;

    public override void OnNetworkSpawn()
    {
        InitializeBlendShape();
    }

    public void LateUpdate()
    {
        if(!IsRendererReady())
        {
            return;
        }
        UpdateBlendShape();
    }

    public void InitializeBlendShape()
    {
        if(blendShapeCount < 1)
        {
            return;
        }

        //player_skinnedMeshRenderer = transform.parent.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        player_skinnedMeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        Debug.Log(player_skinnedMeshRenderer);
        blendShapeValue = new float[blendShapeCount];
        for(int i = 0; i < blendShapeCount; i++)
            blendShapeValue[i] = 0;
    }

    public bool IsRendererReady()
    {
        if(player_skinnedMeshRenderer != null)
        {
            return true;
        }
        return false;
    }

    private void UpdateBlendShape()
    {
        if(!IsOwner) 
        {
            return;
        }
        GetBlendShape();

        UpdateBlendShapeServerRpc(blendShapeValue);

        SetBlendShape(blendShapeValue);
    }

    private void GetBlendShape()
    {
        for (int i = 0; i < blendShapeCount; i++)
        {
            blendShapeValue[i] = player_skinnedMeshRenderer.GetBlendShapeWeight(i);
        }
    }

    private void SetBlendShape(float[] value)
    {
        blendShapeValue = (float[])value.Clone();
        for (int i = 0; i < blendShapeCount; i++)
        {
            player_skinnedMeshRenderer.SetBlendShapeWeight(i, blendShapeValue[i]);
        }
    }

    [ServerRpc]
    private void UpdateBlendShapeServerRpc(float[] value)
    {
        UpdateBlendShapeClientRpc(value);
    }

    [ClientRpc]
    private void UpdateBlendShapeClientRpc(float[] value)
    {
        if (IsOwner) 
        {
            return;
        }

        SetBlendShape(value);
    }
}