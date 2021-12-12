using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectOnTiles : MonoBehaviour {

    void Start() {
        Debug.Log(transform.childCount);

        for (int i = 0; i < transform.childCount; i++) {
            GameObject currentObj = transform.GetChild(i).gameObject;
            Tile tile = GameObject.Find($"Case x:{currentObj.transform.position.x} y:{currentObj.transform.position.z}")?.GetComponent<Tile>();
            if (tile == null)
                continue;
            tile.tileEntity = currentObj?.GetComponent<Entity>();
        }
    }

    void Update() {
        
    }
}
