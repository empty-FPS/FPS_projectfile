using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AimState
{
    Koshi, ADS
}

public class PlayerControl : MonoBehaviour {

    
    public AimState aimState;

    [SerializeField] private float fireRate;//連射速度
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject FPSController;//コンポーネントを取得するために標準アセットのFPSControllerを入れる
    [SerializeField] private GameObject FirstPersonCharacter;//標準アセットのFirstPersonCharacterを入れる
    [SerializeField] private AudioClip Zyuusei;
    [SerializeField] private Camera CameraADS  /*ADSカメラ 独自で存在*/;
    [SerializeField] private Camera CameraKoshi/*腰撃ちカメラ FirstPersonCharacterについてる*/;
    [SerializeField] private int bulletDamage;/*銃弾に付与するダメージ値。装備により変わる*/

    private AudioSource audioPlayer;
    private PhotonView photonview;
    private float time;//計測時間
    private Vector3 recoil;/*リコイルで増加する角度　マウスの位置でカメラの角度が調整されているため、現状は無意味な変数*/
    private bool firstBullet;/*初弾を発射したかどうか*/

    public int ammo;/*弾薬数*/

    // Use this for initialization
    void Start () {
        photonview = GetComponent<PhotonView>();
        audioPlayer = FPSController.GetComponent<AudioSource>();
        fireRate = 0.1f;
        aimState = AimState.Koshi;
        CameraKoshi = FirstPersonCharacter.GetComponent<Camera>();
        ammo = 1000;
        bulletDamage = 3;
        firstBullet = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        //Test();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(aimState == AimState.Koshi)
            {
                CameraADS.enabled = true;
                CameraKoshi.enabled = false;
                aimState = AimState.ADS;
            }
            else
            {
                CameraADS.enabled = false;
                CameraKoshi.enabled = true;
                aimState = AimState.Koshi;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && photonview.isMine && ammo != 0) ShootBullet();

        if (Input.GetKeyUp(KeyCode.Mouse0) && photonview.isMine && ammo != 0)
        {
            firstBullet = false;
            time = 0;
        }
        

        
        
		
	}

    //[PunRPC]
    public void ShootBullet()
    {
        if (!firstBullet)//初弾が撃たれていなければ、初弾を撃つ
        {
            audioPlayer.PlayOneShot(Zyuusei);

            //銃弾をGameObjectとして生成
            GameObject bullet = (GameObject)PhotonNetwork.Instantiate("SphereBullet", transform.position, Quaternion.identity, 0);
            
            //銃弾の角度を調整
            Quaternion direct = muzzle.transform.rotation;
            bullet.transform.rotation = direct;
            
            //銃弾の初期位置を調整
            Vector3 pos = muzzle.transform.position;
            bullet.transform.position = pos;

            //飛ばす
            bullet.GetComponent<BulletController>().shootBullet(300, bullet.transform.forward);
            
            //銃のダメージを代入
            bullet.GetComponent<BulletController>().damage = bulletDamage;
            
            //弾数を減らす
            ammo--;

            firstBullet = true;
        }
        else//初弾を撃っていれば
        {
            time += Time.deltaTime;
            if (time >= fireRate)
            {
                // PhotonNetworkでは、同期したいオブジェクトを生成する場合、PhotonNetwork.Instantiate()を使う必要がある。
                //さらに、Resourceフォルダ内にPrefabが存在している必要があり、"Prefab名"として生成しなければならない

                //銃声を鳴らす
                audioPlayer.PlayOneShot(Zyuusei);

                //銃弾をGameObjectとして生成
                GameObject bullet = (GameObject)PhotonNetwork.Instantiate("SphereBullet", transform.position, Quaternion.identity, 0);
                //銃弾の角度を調整
                Quaternion direct = muzzle.transform.rotation;
                bullet.transform.rotation = direct;
                Vector3 pos = muzzle.transform.position;
                //銃弾の初期位置を調整
                bullet.transform.position = pos;
                //Debug.Log(bullet.transform.position);
                bullet.GetComponent<BulletController>().shootBullet(30, bullet.transform.forward);
                //銃のダメージを代入
                bullet.GetComponent<BulletController>().damage = bulletDamage;
                //bullet.GetComponent<>
                ammo--;
                time = 0;

            }
        }
        
    }

    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("OK");
        if(other.gameObject.tag == "Supply" && photonview.isMine)
        {

            photonview.RPC("GetAmmo", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void GetAmmo()
    {
        //Debug.Log("OK");
        ammo = 30;
    }

    private void Test()/*いろいろとテストするための関数　任意で内容変更*/
    {
        float a = Input.GetAxis("Mouse ScrollWheel");
        if(a != 0)
        {
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

        }
    }


}
