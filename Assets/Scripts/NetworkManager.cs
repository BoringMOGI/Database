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
        // ���� ������ ������ ����.
        PhotonNetwork.ConnectUsingSettings(PhotonNetwork.gameVersion);
        stateText.text = "������ ������...";
    }

    public override void OnConnectedToMaster()
    {
        stateText.text = "������ ������ ���� ����!";
        PhotonNetwork.JoinRandomRoom();

    }
    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("������ ������ ���� ����");
    }


    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        stateText.text = "���� �뿡 ������ �����߽��ϴ�.";
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 2 }, new TypedLobby());
    }

    public override void OnCreatedRoom()
    {
        stateText.text = "���� �����߽��ϴ�.\n";
    }
    public override void OnJoinedRoom()
    {
        // TestRoom�� ���� �÷��̾ �Ҹ��� �ݹ� �Լ�.
        // ��� �� �����ڰ� Main ���� �ε��Ѵ�.
        PhotonNetwork.LoadLevel("Main");
    }

}
