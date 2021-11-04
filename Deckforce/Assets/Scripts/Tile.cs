using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Entity tileEntity;
    public Vector2 tilePosition;

    // Start is called before the first frame update
    void Start()
    {
        tilePosition = new Vector2(transform.position.x, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
