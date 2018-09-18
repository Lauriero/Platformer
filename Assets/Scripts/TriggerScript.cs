using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

    private Movement _m;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	
	}

    //void OnTriggerEnter2D(Collider2D col) {
    //    if (col.gameObject.tag == "Player") {

    //        _m = col.gameObject.GetComponent<Movement>();

    //        if (_m.Direction == 1) {
    //            print("Left");
    //            _m.TpToward = new Vector3(transform.position.x - 0.64f, col.transform.position.y, col.transform.position.z);
    //        } else if (_m.Direction == -1) {
    //            print("Right");
    //            _m.TpToward = new Vector3(transform.position.x + 0.64f, col.transform.position.y, col.transform.position.z);
    //        } else if () {
    //            print("Up");
    //            _m.TpToward = new Vector3(col.transform.position.x, transform.position.y - 0.64f, col.transform.position.z);
    //        } else if (_m.Direction == -2) {
    //            print("Down");
    //            _m.TpToward = new Vector3(col.transform.position.x, transform.position.y + 0.64f, col.transform.position.z);
    //        }
    //    }
    //}

    //void OnTriggerStay2D(Collider2D col) {
    //    if (col.gameObject.tag == "Player") {
    //        _m = col.gameObject.GetComponent<Movement>();

  
    //    }
    //}


    //void OnTriggerExit2D(Collider2D col) {
    //     if (col.gameObject.tag == "Player") {
    //        _m.canMoveUp = true;
    //        _m.canMoveDown = true;
    //        _m.canMoveLeft = true;
    //        _m.canMoveRight = true;
    //    }
    //}
}
