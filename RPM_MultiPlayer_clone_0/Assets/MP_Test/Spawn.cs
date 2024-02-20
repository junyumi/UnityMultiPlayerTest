using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkObject))]
public class Spawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    public static string InputName = string.Empty;
    //private var networkPrefabList = null;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;
    }

    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsHost && sceneName == "AvatarControl_HY 3_Spawn 1")
        {
            foreach (var id in clientsCompleted)
            {
                var player = Instantiate(playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
                
            }
        }
    }

    private void FindAvatar()
    {
        //var info = networkPrefabList.Find(a => a.Prefab.name == InputName);

    }

    public void Initialize()
    {
        //networkPrefabList = NetworkManager.NetworkConfig.Prefabs.NetworkPrefabsLists.Find(l => l.name == "Multi Play XR_Avatar Repository NetworkPrefabsList").PrefabList.ToList();
    }
}
