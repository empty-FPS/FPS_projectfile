using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHowBig : MonoBehaviour {


    [SerializeField] private GameObject targetObject;

    private float size_x , size_y , size_z;//x,y,z成分の長さ

	// Use this for initialization
	void Start () {
        //大きさの取得対象を自身に設定
        targetObject = gameObject;
       size_y = targetObject.GetComponent<Renderer>().bounds.size.y;
       size_x = targetObject.GetComponent<Renderer>().bounds.size.x;
        size_z = targetObject.GetComponent<Renderer>().bounds.size.z;
        Debug.Log("x：" + size_x + "&　y：" + size_y + "& z：" + size_z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
