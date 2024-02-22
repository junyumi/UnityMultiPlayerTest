using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class VivoxPlayer : MonoBehaviour
{
    private VivoxParticipant localParticipant;
    
  // 로그인 기능
    public GameObject button;
    
    private int PermissionAskedCount;

    [SerializeField] public string VoiceChannelName = "Yumichannel";
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        VivoxService.Instance.LoggedIn += OnUserLoggedIn;
        VivoxService.Instance.LoggedOut += OnUserLoggedOut;
        
        VivoxService.Instance.ParticipantAddedToChannel += OnParticipantAdded;
        VivoxService.Instance.ParticipantRemovedFromChannel += OnParticipantRemoved;
        VivoxService.Instance.LoggedOut += OnUserLoggedOut;
        VivoxService.Instance.ChannelLeft += OnChannelDisconnected;


        if (button != null) button.GetComponent<Button>().onClick.AddListener(() => LoginToVivoxService());
    }

  
    private void OnDestroy()
    {
        VivoxService.Instance.LoggedIn -= OnUserLoggedIn;
        VivoxService.Instance.LoggedOut -= OnUserLoggedOut;
        
        VivoxService.Instance.ParticipantAddedToChannel -= OnParticipantAdded;
        VivoxService.Instance.ParticipantRemovedFromChannel -= OnParticipantRemoved;
        VivoxService.Instance.LoggedOut -= OnUserLoggedOut;
        VivoxService.Instance.ChannelLeft -= OnChannelDisconnected;
    }

    void LoginToVivoxService()
    {
        if (IsMicPermissionGranted())
        {
            // The user authorized use of the microphone.
            LoginToVivox();
        }
        else
        {
            // We do not have the needed permissions.
            // Ask for permissions or proceed without the functionality enabled if they were denied by the user
            if (IsPermissionsDenied())
            {
                PermissionAskedCount = 0;
                LoginToVivox();
            }
            else
            {
                AskForPermissions();
            }
        }

       button.SetActive(false);

    }
    
    async void LoginToVivox()
    {
       
        var loginOptions = new LoginOptions()
        {
            DisplayName = NetworkManager.Singleton.LocalClientId.ToString(),
            ParticipantUpdateFrequency = ParticipantPropertyUpdateFrequency.FivePerSecond,

        };
        await VivoxService.Instance.LoginAsync(loginOptions);
        Debug.Log(loginOptions.DisplayName);
        
    }
    
    bool IsMicPermissionGranted()
    {
        bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp())
        {
            // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission for all features to work
            isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
        }
#endif
        return isGranted;
    }

    void AskForPermissions()
    {
        string permissionCode = Permission.Microphone;
        
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (m_PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
        

        PermissionAskedCount++;
        Permission.RequestUserPermission(permissionCode);
    }
    
    bool IsPermissionsDenied()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
        if (IsAndroid12AndUp())
        {
            return m_PermissionAskedCount == 2;
        }
#endif
        return PermissionAskedCount == 1;
    }
    

    async void OnUserLoggedOut()
    {
       
    }

    async void OnUserLoggedIn()
    {
        Debug.Log("LOGGED" + VivoxService.Instance.ActiveChannels.Count);
        await JoinLobbyChannel();
        Debug.Log("LOGGED with channel" + VivoxVoiceManager.LobbyChannelName+ " , "+VivoxService.Instance.ActiveChannels.FirstOrDefault().ToString());

        SuccessToLogin();
        
    }


    private void SuccessToLogin()
    {
        if (VivoxService.Instance.ActiveChannels.Count > 0)
        {
            Debug.Log("--"+VivoxService.Instance.ActiveChannels.Count);
            var channel = VivoxService.Instance.ActiveChannels.FirstOrDefault();
            
            localParticipant = channel.Value.FirstOrDefault(p => p.IsSelf);

            //Debug.Log("local "+localParticipant.PlayerId + " audio"+localParticipant.AudioEnergy);

            localParticipant.ParticipantSpeechDetected += OnSpeechDetected;
            localParticipant.ParticipantAudioEnergyChanged += OnEnergyDetected;

        }
    }

    private void OnChannelDisconnected(string obj)
    {
        Debug.Log("Discconncted to "+obj);
    }

    private void OnParticipantRemoved(VivoxParticipant obj)
    {
        Debug.Log("remove "+ obj.DisplayName+" on "+VivoxVoiceManager.LobbyChannelName);
    }

    private void OnParticipantAdded(VivoxParticipant obj)
    {
        Debug.Log("add "+ obj.DisplayName+" on  "+VivoxVoiceManager.LobbyChannelName);
    }

    
    Task JoinLobbyChannel()
    {
        return VivoxService.Instance.JoinGroupChannelAsync(VivoxVoiceManager.LobbyChannelName, ChatCapability.TextAndAudio);
    }
    
    void OnEnergyDetected()
    {
        
        Debug.Log(NetworkManager.Singleton.LocalClientId+"EnergyDetected "+localParticipant.AudioEnergy);
       
        
    }
    
    /*
     *  audio 스피치 가 되는지 안되는지
     * 말 하면 true 안하면 false 
     */
    void OnSpeechDetected()
    {
        Debug.Log("speechDetected "+localParticipant.SpeechDetected);
        if (localParticipant.SpeechDetected)
        {
            //_avatarControl.c3AvatarGazing();
        }
        
    }
}
