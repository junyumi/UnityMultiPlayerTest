using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class Character: ScriptableObject
{

    [SerializeField] private int id = -1;

    [SerializeField] private string displayName = "New Display Name";

    [SerializeField] private Sprite icon;

    [SerializeField] private GameObject introPrefab;
    
    [SerializeField] private NetworkObject gameplayerPrefab;

    public int Id => id;

    public string DisplayName => displayName;

    public Sprite Icon => icon;

    public GameObject IntroPrefab => introPrefab;

    public NetworkObject GamePlayerPrefab => gameplayerPrefab;
    
}
