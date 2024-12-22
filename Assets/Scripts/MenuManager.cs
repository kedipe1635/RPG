using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine;
public class MenuManager : MonoBehaviourPunCallbacks

{
    [SerializeField] TMP_Text logText;
    [SerializeField] TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        // Oyuncuya rastgele sayý etiketli bir kullanýcý adý verelim
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
        // Log yazý alanýnda bu kullanýcý adýný gösterelim
        Log("Player Name: " + PhotonNetwork.NickName);
        // Oyun ayarlarýný yapalým
        PhotonNetwork.AutomaticallySyncScene = true; // Pencereler arasýndaki otomatik geçiþ
        PhotonNetwork.GameVersion = "1"; // Oyun versiyonunu ayarlama
        PhotonNetwork.ConnectUsingSettings(); // Photon sunucusuna baðlanma
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Log(string message)
    {
        logText.text += "\n";
        logText.text += message;
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnConnectedToMaster()
    {
        Log("Connected to the server");
    }
    public override void OnJoinedRoom()
    {
        Log("Joined the lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void ChangeName()
    {
        //InputField alanýna yazýlan yazýyý okumak
        PhotonNetwork.NickName = inputField.text;
        //Yeni kullanýcý adýný çýktý vermek:
        Log("New Player name: " + PhotonNetwork.NickName);
    }

}
