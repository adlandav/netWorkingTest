using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainMenue : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject findOpponnentPanel = null;
    [SerializeField] private GameObject waitingStatusPanel = null;
    [SerializeField] private TextMeshProUGUI waitingStatusText = null;

    private bool isConnecting = false;
    private const string GameVersion = "0.1"; //Change with the gameVersion
    private const int MaxPlayersPerRoom = 2;

    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    public void FindOpponent() {
        isConnecting = true;

        findOpponnentPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);

        waitingStatusText.text = "searching...";

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        if (isConnecting) {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatusPanel.SetActive(false);
        findOpponnentPanel.SetActive(true);

        Debug.Log($"Disconnected due to {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients waiting, creating new room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("client joined the room succesfully");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount != MaxPlayersPerRoom)
        {
            waitingStatusText.text = "waiting for opponent";

            Debug.Log("client is waiting for opponent");
        }
        else {
            waitingStatusText.text = "opponent found";
            Debug.Log("match is ready to begin");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom) {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            waitingStatusText.text = "opponent found";
            Debug.Log("Match is ready to begin");

            PhotonNetwork.LoadLevel("testScene");
        }
    }
}