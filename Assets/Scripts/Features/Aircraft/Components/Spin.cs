using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 Axis;
    public float speed = 20;

    void Update()
    {
        transform.Rotate(Axis * (speed * Time.deltaTime), Space.Self);
    }
}