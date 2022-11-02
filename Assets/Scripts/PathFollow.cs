using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Transform[] target;
    public float speed;
    private int current;
    public bool Move = false;

    void Update()
    {
        if(current >= target.Length)
        {
            speed = 0;
            current = target.Length;
        }

        if (current < target.Length && Move)
        {
            if (transform.position != target[current].position)
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos);
            }
            else current = (current + 1); // add this line to make a loop. % target.Length;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Move = true;
        }
    }
}
