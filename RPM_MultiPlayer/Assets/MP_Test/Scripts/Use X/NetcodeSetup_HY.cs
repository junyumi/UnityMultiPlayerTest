using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class NetcodeSetup_HY : MonoBehaviour
    {
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        [SerializeField] private GameObject startPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private InputField nameField;
        [SerializeField] private TMP_Text log;

        private Dictionary<string, string> avatarList = new Dictionary<string, string>()
        {
            {"1","https://models.readyplayer.me/65abc273e969ba4048cecaf4.glb"},
            {"2", "https://models.readyplayer.me/65b0c1cdceb9f8934201ba76.glb"},
            {"3", "https://models.readyplayer.me/65abc54be84926e3d66a1d78.glb"}
        };


        private void Start()
        {
            log.text = "Error Log";

            hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
            clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
            startButton.onClick.AddListener(() =>
            {
                if(avatarList.ContainsKey(nameField.text)){
                    var url = avatarList[nameField.text];
                    NetworkPlayer.InputUrl = url;
                    
                    startPanel.SetActive(false);
                    connectionPanel.SetActive(true);
                }
                else
                {
                    log.text = "There is not the avatar prefab.";
                    return;
                }

            });
            
            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                connectionPanel.SetActive(false);
            };
        }
    }
}

