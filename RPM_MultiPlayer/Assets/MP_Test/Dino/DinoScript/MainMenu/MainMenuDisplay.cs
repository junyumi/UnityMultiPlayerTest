using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

    public class MainMenuDisplay : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private GameObject connectingPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private TMP_InputField joinCodeInputField;
        private async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }
            
            connectingPanel.SetActive(false);
            menuPanel.SetActive(true);

            StartHost();
            StartClient();
        }

        public void StartHost()
        {
            hostButton.onClick.AddListener(() =>
            {
                Debug.Log("start host");
                
                HostManager.Instance.StartHost();
                
            });
        }
        
        public void StartClient()
        {
            clientButton.onClick.AddListener(async () =>
            {
                Debug.Log("start client");
                await ClientManager.Instance.StartClient(joinCodeInputField.text);
            });
        }
        
    }

