using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**銃弾そのものを制御するスクリプト
 */
public class BulletController : MonoBehaviour {


    [SerializeField] private GameObject player;/*銃弾を発射したプレイヤーを格納*/

    private Rigidbody rb;
    private float time;
    private float distance;
    private PhotonView photonView;
    private Vector3 pos;//銃弾の初期位置

    public int damage;/*銃弾のダメージ　プレイヤーが装備している武器により値が変わる*/
    public float distLimit;


	void Start () {
        photonView = GetComponent<PhotonView>();
        pos = transform.position;
        distLimit = 10;
	}
	
	void Update () {
        //現在位置を代入
        Vector3 nowPos = transform.position;
        //距離を計測
        distance = Vector3.Magnitude(transform.position - pos);

        if(distance >= distLimit && photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        
	}

    public void shootBullet(float power , Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(power * direction , ForceMode.Impulse);
    }

    


}
