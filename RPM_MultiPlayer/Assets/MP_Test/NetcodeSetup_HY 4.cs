using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class NetcodeSetup_HY4 : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button serverButton;
        [SerializeField] private string gameplaySceneName = "Gameplay";
        
        private void Start()
        {
            hostButton.onClick.AddListener(() =>
            {
                Debug.Log("start host");
                
                ServerManager.Instance.StartHost();
                
            });

            serverButton.onClick.AddListener(() =>
            {
                Debug.Log("start server");

                ServerManager.Instance.StartServer();
                

            });

            clientButton.onClick.AddListener(() =>
            {
                Debug.Log("start client");
                NetworkManager.Singleton.StartClient();
            });
            
        }
    }
}

