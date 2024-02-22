using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class NetcodeSetup_HY4 : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button serverButton;
        [SerializeField] private GameObject connectingPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private TMP_InputField joinCodeInputField;

        private VivoxPlayerManager _vivoxPlayer;
        private void Start()
        {
            hostButton.onClick.AddListener(() =>
            {
                Debug.Log("start host");
                
                HostManager.Instance.StartHost();
                
                _vivoxPlayer.LoginToVivoxService();
                

            });

            clientButton.onClick.AddListener(async () =>
            {
                Debug.Log("start client");
                await ClientManager.Instance.StartClient(joinCodeInputField.text);
                
                _vivoxPlayer.LoginToVivoxService();
            });
            
        }
    }
}

