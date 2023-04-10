using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    [SerializeField]
    private GameObject sceneCamera;

    public static GameManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            return;
        }

        Debug.LogError("Sahnede birden fazla GameManager örneði");
    }
    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
        {
            return;
        }

        sceneCamera.SetActive(isActive);
    }

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }


    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {

        return players[playerId];
    }


}
