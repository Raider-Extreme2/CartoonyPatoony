using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] Transform lookTarget;
    [SerializeField] float attackDistance;
    [SerializeField] float enemySpeed;
    [SerializeField] float detectionRange;

    [Header("Animation")]
    [SerializeField] Animator enemyAnimator;

    [Header("Life")]
    public int vida;
    bool isAlive;

    [Header("Attack")]
    [SerializeField] BoxCollider slappyHand;
    [SerializeField] int damage;

    private void Start()
    {
        isAlive = true;
    }
    private void Update()
    {
        if (!isAlive) return;
        
            if (vida <= 0)
            {
                StartCoroutine(EnemyDeath());
            }

            agent.SetDestination(target.position);
            transform.LookAt(new Vector3(lookTarget.position.x, transform.position.y, lookTarget.position.z));


            if (agent.remainingDistance > detectionRange)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }

            if (agent.remainingDistance <= attackDistance)
            {
                enemyAnimator.SetBool("CocoAttack", true);
            }
            else
            {
                enemyAnimator.SetBool("CocoAttack", false);
            }

            if (agent.velocity.magnitude > 0.1f)
            {
                enemyAnimator.SetBool("CocoWalk", true);
                Debug.Log("estou andando a " + agent.velocity.magnitude + " por segundo");
            }
            else
            {
                enemyAnimator.SetBool("CocoWalk", false);
            }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerControler>();
        if (player != null)
        {
            player.healt -= damage;
        }
    }

    IEnumerator EnemyDeath() 
    {
        isAlive = false;
        agent.isStopped = true;
        enemyAnimator.SetBool("CocoDeath", true);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
