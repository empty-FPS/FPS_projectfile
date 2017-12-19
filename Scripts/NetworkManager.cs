using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {


    private bool IsLeader;/*ルームを作った人間かどうか*/
    private bool GameIsRunning;/*ゲーム中かどうか*/


    void Start()
    {
        // Photonに接続する(引数でゲームのバージョンを指定できる)
        PhotonNetwork.ConnectUsingSettings(null);
        IsLeader = false;
        GameIsRunning = false;
    }

    // ロビーに入ると呼ばれる
    void OnJoinedLobby()
    {
        Debug.Log("ロビーに入りました。");

        // ルームに入室する
        PhotonNetwork.JoinRandomRoom();
    }

    // ルームに入室すると呼ばれる
    void OnJoinedRoom()
    {
        Debug.Log("ルームへ入室しました。");

        //プレイヤーを生成
        Vector3 point = new Vector3(Random.Range(-1, 1), 1.5f, Random.Range(-1, 1));
        GameObject player = (GameObject)PhotonNetwork.Instantiate("FPSController", point , Quaternion.identity , 0);
    }

    // ルームの入室に失敗すると呼ばれる
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("ルームの入室に失敗しました。");

        // ルームがないと入室に失敗するため、その時は自分で作る
        // 引数でルーム名を指定できる
        PhotonNetwork.CreateRoom("myRoomName");
        IsLeader = true;
    }

    void Update () {
        if (IsLeader && !GameIsRunning)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                //敵を生成
                StartCoroutine(SpawnEnemies(10));
                //ゲーム開始
                GameIsRunning = true;
            }
        }
    }

    public IEnumerator SpawnEnemies(int count)
    {
        for (int a = 0; a < count; a++)
        {
            //敵を生成
            GameObject[] enemySpawnPos = GameObject.FindGameObjectsWithTag("EnemySpawnPos");
            //Debug.Log(enemySpawnPos.Length);
            int b = Random.Range(0, enemySpawnPos.Length);
            GameObject enemy = (GameObject)PhotonNetwork.Instantiate("CapsuleBody", enemySpawnPos[b].transform.position, Quaternion.identity, 0);
            yield return new WaitForSeconds(1.0f);
        }
    }

    
}
