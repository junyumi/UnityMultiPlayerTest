using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class NetcodeSetup_HY3 : NetworkBehaviour
    {
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        [SerializeField] private GameObject startPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private InputField indexField;
        [SerializeField] private TMP_Text log;

        private Dictionary<string, string> avatarList = new Dictionary<string, string>()
        {
            {"1","Female_Subject"},
            {"2", "Female_Researcher1"},
            {"3", "Male_Subject"}
        };


        private void Start()
        {
            log.text = "Error Log";

            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
                StartGame();
            });
            clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
            startButton.onClick.AddListener(() =>
            {
                if(avatarList.ContainsKey(indexField.text)){
                    //var avatarName = avatarList[indexField.text];
                    //Spawn.InputName = avatarName;
                    
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

        public void StartGame()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("AvatarControl_HY 3_Spawn 1", LoadSceneMode.Single);
            }
                
        }
    }
}

