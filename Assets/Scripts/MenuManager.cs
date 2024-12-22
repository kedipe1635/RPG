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
        // Oyuncuya rastgele say� etiketli bir kullan�c� ad� verelim
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
        // Log yaz� alan�nda bu kullan�c� ad�n� g�sterelim
        Log("Player Name: " + PhotonNetwork.NickName);
        // Oyun ayarlar�n� yapal�m
        PhotonNetwork.AutomaticallySyncScene = true; // Pencereler aras�ndaki otomatik ge�i�
        PhotonNetwork.GameVersion = "1"; // Oyun versiyonunu ayarlama
        PhotonNetwork.ConnectUsingSettings(); // Photon sunucusuna ba�lanma
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
        //InputField alan�na yaz�lan yaz�y� okumak
        PhotonNetwork.NickName = inputField.text;
        //Yeni kullan�c� ad�n� ��kt� vermek:
        Log("New Player name: " + PhotonNetwork.NickName);
    }

}
