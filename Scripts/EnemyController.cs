using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//メモ欄
/**・敵の部位ごとにダメージを分けるため、部位のオブジェクトに対し、それに応じたタグ(Body,Head,Leg,Arm)を付ける。
 * ・それゆえに、ダメージの計算は敵がしたほうがいい
 * ・ダメージ倍率
 *      弱点　　：2倍
 *      それ以外：1倍
 */

public abstract class EnemyController : MonoBehaviour {

    
    
    [SerializeField] private GameObject enemyBody;/*このゲームオブジェクトの体力を敵の総合体力とする*/
    [SerializeField] private float speed ;

    private int money;/*倒したときにもらえる金*/
    private GameObject targetObject;
    private Transform player;

    public int e_hp;/*敵の体力*/

    //public int[] hpPerPart;

    Dictionary<string, int> hpPerPart = new Dictionary<string, int>();

    [SerializeField] private Material[] enemyColor;



    void Start () {
        //enemyBody = gameObject;
        //speed = Random.Range(2.1f, 4.5f);
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //int a = Random.Range(0, players.Length);
        //targetObject = players[a];
        //player = targetObject.transform;
        //transform.LookAt(player);
        //e_hp = 9;

        //hpPerPart.Add("Head", 6);
        //hpPerPart.Add("Body", 9);
        //hpPerPart.Add("Leg", 3);
        //hpPerPart.Add("Arm", 3);

	}
	
	
	void Update () {

        //if(gameObject.tag == "Body")
        //{
        //    /*プレイヤーを追尾*/
        //    //プレイヤーの位置を取得
        //    Vector3 playerPos = player.position;
        //    //自身とプレイヤーの位置から、進行すべき方向を算出
        //    Vector3 direction = playerPos - transform.position;


        //    direction = direction.normalized;
        //    Vector3 targetPos = transform.position + (direction * speed * Time.deltaTime);

        //    //プレイヤーがジャンプしたときに、敵が浮かないように敵のy座標を固定
        //    targetPos.y = 2.5f;
        //    transform.position = targetPos;
        //    //プレイヤーの方向を向く
        //    transform.LookAt(player);

        //    /*x軸とz軸を固定*/
        //    Quaternion enemyRotation = transform.rotation;
        //    enemyRotation.x = 0;
        //    enemyRotation.z = 0;
        //    transform.rotation = enemyRotation;
        //}
        




	}

    
    public void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log(gameObject.tag);
        if(collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(ChangeEnemyColor(1.0f, collision.gameObject));
            CalcDamage(gameObject, collision.gameObject.GetComponent<BulletController>().damage);
          //  //Debug.Log(gameObject.tag);
          //  switch (gameObject.tag)
          //  {
          //      case "Body":

          //          break;
          //  }
          //e_hp -= collision.gameObject.GetComponent<BulletController>().damage;
          //if(e_hp <= 0)
          //{
          //    PhotonNetwork.Destroy(gameObject);
          //}
        }
        PhotonNetwork.Destroy(collision.gameObject);
    }


    //public abstract void CalcDamage(GameObject hitObj);//オーバーライドで中身を変える
    
    public virtual void CalcDamage(GameObject hitObj , int buletDamage)
    {
        Debug.Log(hitObj.tag);
        int damage = buletDamage;

        //ダメージ計算分岐　ここ
        
        hpPerPart[hitObj.tag] -= damage;
        if(hpPerPart[hitObj.tag] <= 0)
        {
            PhotonNetwork.Destroy(hitObj);
        }

    }

    public IEnumerator ChangeEnemyColor(float changeTime , GameObject hitObj)
    {
        if(hitObj != null)
        {
            hitObj.GetComponent<MeshRenderer>().material = enemyColor[1];
            yield return new WaitForSeconds(changeTime);
            hitObj.GetComponent<MeshRenderer>().material = enemyColor[0];
        }
        
    }

    public void Init()
    {
        enemyBody = gameObject;
        speed = Random.Range(2.1f, 4.5f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int a = Random.Range(0, players.Length);
        targetObject = players[a];
        player = targetObject.transform;
        transform.LookAt(player);
        e_hp = 9;

        hpPerPart.Add("Head", 6);
        hpPerPart.Add("Body", 9);
        hpPerPart.Add("Leg", 3);
        hpPerPart.Add("Arm", 3);
    }

    public void Move()
    {
        //if (gameObject.tag == "Body")
        //{
            /*プレイヤーを追尾*/
            //プレイヤーの位置を取得
            Vector3 playerPos = player.position;
            //自身とプレイヤーの位置から、進行すべき方向を算出
            Vector3 direction = playerPos - transform.position;


            direction = direction.normalized;
            Vector3 targetPos = transform.position + (direction * speed * Time.deltaTime);

            //プレイヤーがジャンプしたときに、敵が浮かないように敵のy座標を固定
            targetPos.y = 2.5f;
            transform.position = targetPos;
            //プレイヤーの方向を向く
            transform.LookAt(player);

            /*x軸とz軸を固定*/
            Quaternion enemyRotation = transform.rotation;
            enemyRotation.x = 0;
            enemyRotation.z = 0;
            transform.rotation = enemyRotation;
        Console.WriteLine("tuiki");
        //}
    }
}
//switch (hitObj.tag)
//        {
//            case "Body":

//                break;
//            case "Leg":

//                break;
//            case "Arm":

//                break;
//            case "Head":

//                break;
//        }