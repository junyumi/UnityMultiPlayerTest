using System;
using UnityEngine;
//Client 구조체


[Serializable]
public class ClientData
{
    public ulong ClientId;

    public int characterId = -1;

    public ClientData(ulong clientId)
    {
        this.ClientId = clientId;
    }
}
