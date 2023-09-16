using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public Vector3 Axis;
    public float speed = 20;
	
	// Update is called once per frame
	void Update ()
    {
 
        transform.Rotate(Axis * (speed * Time.deltaTime), Space.Self);
	}
}
