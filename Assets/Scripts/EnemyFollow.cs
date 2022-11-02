using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    
    public Transform Player;
    public NavMeshAgent Agent;
    public bool InRange = false;
    public float ColliderSmall;
    private float ColliderNormal;
    private PlayerMovement PlayerMovement;

    private void Start()
    {
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        ColliderNormal = GetComponent<SphereCollider>().radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            InRange = false;
        }
    }

    private void Update()
    {
        if (PlayerMovement.Hide)
        {
            GetComponent<SphereCollider>().radius = ColliderSmall;
        }
        else GetComponent<SphereCollider>().radius = ColliderNormal;

        if (InRange)
        {
            Agent.isStopped = false;
            Agent.SetDestination(Player.position);
        }
        else Agent.isStopped = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Dead = true;
            print("Player collision with enemy");
        }
    }
}
