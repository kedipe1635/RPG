using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] TMP_Text PlayersText;
    [SerializeField] GameObject startButton;


    void Start()
    {
        RefreshPlayers();
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Log(newPlayer.NickName + " entered the room");
        RefreshPlayers();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Log(otherPlayer.NickName + " left the room");
        RefreshPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    void Update()
    {

    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    void Log(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    [PunRPC]
    public void ShowMessage(string message, PhotonMessageInfo info)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }
    public void Send()
    {
        if (string.IsNullOrWhiteSpace(InputText.text)) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            InputText.text = string.Empty;
        }
    }
    void RefreshPlayers()
    {
        // �a�r� yaln�zca Ana �stemci (sunucuyu olu�turan oyuncu) taraf�ndan yap�labilir
        if (PhotonNetwork.IsMasterClient)
        {
            // Lobideki t�m oyuncular i�in ShowPlayers y�ntemini �a��rma
            photonView.RPC("ShowPlayers", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ShowPlayers()
    {
        // Oyuncu listesini temizleme, sadece 'Players:' sat�r�n� b�rakma
        PlayersText.text = "Players: ";
        // Sunucudaki t�m oyuncular�n i�in �al��acak bir d�ng� ba�latma
        foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
        {
            // Sonraki sat�ra ge�i�
            PlayersText.text += "\n";
            // Kullan�c� ad�n� ��kt� vermek
            PlayersText.text += otherPlayer.NickName;
        }
    }

    public void StartButton()
    {
        PhotonNetwork.LoadLevel("Game");

    }
}