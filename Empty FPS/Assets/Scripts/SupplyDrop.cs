using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour {

    private Vector3 onGroundPosition;/*地面についているときのポジション*/
	




	void Start () {
        onGroundPosition = transform.position;
        onGroundPosition.y = 1.5f;
        transform.position = onGroundPosition;
	}
	
	void Update () {
        
    }

    


}
