using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ReadyPlayerMe.Core;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour
{
    // 싱글 톤
    
    [Header("Settings")]
    [SerializeField] private string gameplaySceneName = "GamePlay";
    [SerializeField] private string characterSelectSceneName = "CharacterSelect";
    
    
    public static ServerManager Instance { get; private set; }

    private bool gameHasStarted;
    
    public Dictionary<ulong,ClientData> ClientData { get; private set; }
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartServer()
    {
        //연결 조건에 맞는지 확인하는
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;

        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
        
        NetworkManager.Singleton.StartServer();
    }



    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, ClientData>();
        
        NetworkManager.Singleton.StartHost();
    }
    
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest arg1, NetworkManager.ConnectionApprovalResponse arg2)
    {
        if (ClientData.Count >= 3 || gameHasStarted)
        {
            // Approval Check 입증 시 실패 조건
            arg2.Approved = false;
            return;
        }

        arg2.Approved = true;
        arg2.CreatePlayerObject = false;
        arg2.Pending = false;

        // 조건에 충족하면 접속한 Client ID 로 Client 객체를 만든다.
        ClientData[arg1.ClientNetworkId] = new ClientData(arg1.ClientNetworkId);
        Debug.Log($"add client {arg1.ClientNetworkId} and sum of the Client is {ClientData.Count} and {ClientData.Keys}");}
    
    
    private void OnNetworkReady()
    {
        // 서버에서 클라이언트가 서버에서 연결을 끊었을 때마다 호출되는 이벤트
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        
        // 캐릭터 Select 씬으로 넘어감
        NetworkManager.Singleton.SceneManager.LoadScene(characterSelectSceneName, LoadSceneMode.Single);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (ClientData.ContainsKey(clientId))
        {
            if(ClientData.Remove(clientId))
                Debug.Log($"Remove client {clientId}");
        }
    }

    public void SetCharacter(ulong Clientid, int CharacterId)
    {
        /*
         * TryGetValue(TKey key, out TValue value);
         * key: 찾고자 하는 요소의 키
         * value : key 에 맞는 value 값이 잆으면 ClinetData 데이터 형식으로 가져오고, 찾지 못하면 기본값으로 설정
         */
        if (ClientData.TryGetValue(Clientid, out ClientData data))
        {
            data.characterId = CharacterId;
        }
        
    }

    public void StartGame()
    {
        gameHasStarted = true;

        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
