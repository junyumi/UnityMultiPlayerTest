using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxLoginManager : MonoBehaviour
{
    [Header("VivoxChannelName")]
    [SerializeField]private string channelToJoin = "Lobby";
    // Start is called before the first frame update

    private void Start()
    {
        GetLoginDisplayName();
        
        
        VivoxService.Instance.LoggedIn += OnUserLoggedIn;
        VivoxService.Instance.LoggedOut += OnUserLoggedOut;
      
        
    }

  
    private void OnEnable()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void GetLoginDisplayName()
    {
     //   Debug.Log("sum"+ServerManager.Instance.ClientData.Count);
        foreach (var v in ServerManager.Instance.ClientData)
        {
            if (ServerManager.Instance.ClientData == null)
            {
                return;
            }
            
            //1. 접속한 클라이언트 데이터를 v 에 가져온다.
            
            //2. LoginOption 의 displayname 에 집어넣는다.
            // display name is equal as the client id
            LoginOptions loginOptions = new LoginOptions();
            loginOptions.DisplayName = v.Value.ClientId.ToString();
            loginOptions.EnableTTS = true;

            Debug.Log("login options"+loginOptions.PlayerId + " , "+loginOptions.DisplayName);
            
            await VivoxService.Instance.InitializeAsync();
            
            Debug.Log("Sucess in enter to vivox");
            
            await VivoxService.Instance.LoginAsync(loginOptions);
            
            Debug.Log($"login in {loginOptions.DisplayName}");
            
        }
    }

    async void OnUserLoggedIn()
    {
        await JoinLobbyChannel();
        Debug.Log($"log in success {NetworkManager.ServerClientId} join in {channelToJoin} ");
    }
    
    private void OnUserLoggedOut()
    {
        throw new NotImplementedException();
    }

    Task JoinLobbyChannel()
    {
        return VivoxService.Instance.JoinGroupChannelAsync(channelToJoin, ChatCapability.TextAndAudio);
    }

 

}
