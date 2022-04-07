using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : Photon.PunBehaviour
{
    [SerializeField] Text stateText;

    public void OnStartNetwork()
    {
        // 포톤 마스터 서버에 접속.
        PhotonNetwork.ConnectUsingSettings(PhotonNetwork.gameVersion);
        stateText.text = "서버에 접속중...";
    }

    public override void OnConnectedToMaster()
    {
        stateText.text = "마스터 서버에 접속 성공!";
        PhotonNetwork.JoinRandomRoom();

    }
    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("마스터 서버에 접속 실패");
    }


    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        stateText.text = "랜덤 룸에 입장을 실패했습니다.";
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 2 }, new TypedLobby());
    }

    public override void OnCreatedRoom()
    {
        stateText.text = "방을 생성했습니다.\n";
    }
    public override void OnJoinedRoom()
    {
        // TestRoom에 들어온 플레이어가 불리는 콜백 함수.
        // 모든 룸 참가자가 Main 씬을 로드한다.
        PhotonNetwork.LoadLevel("Main");
    }

}
