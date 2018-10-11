using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gObject : MonoBehaviour {

    public void Generate(GameObject pref, int x, int y, int z, Sprite s) {
        GameObject obj = Instantiate(pref);
        obj.GetComponent<Transform>().position = new Vector3(x, y, z);
        obj.GetComponent<SpriteRenderer>().sprite = s;
    }

    public void Collide() {
        
    }

    public void startInteraction() {

    }


}
