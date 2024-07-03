using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    [SerializeField] Vector3 _min, _max;
    [SerializeField] float _playerDetectionDistance, _playerAttackDistance;
    Vector3 _destination;
    Transform _player;

    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /**
     * By default the enemies are motionless.
     * When the status of the world reaches 20%, the enemies start patrolling.
     * When the status of the world reaches 50%, the enemies start being in alert.
     * When the status of the world reaches 70%, the enemies start going crazy.
     */
    void Update() {
        if (!GameManager.instance.isActive) StopAllCoroutines();
    }

    void RandomDestination() {
        _destination = new Vector3(Random.Range(_min.x, _max.x), 0, Random.Range(_min.z, _max.z));
        GetComponent<NavMeshAgent>().SetDestination(_destination);
        GetComponent<Animator>().SetFloat("velocity", 2);
    }

    IEnumerator Patrol() {
        GetComponent<NavMeshAgent>().SetDestination(_destination);
        while (true) {
            if (Vector3.Distance(transform.position, _destination) < 1.5f) {
                GetComponent<Animator>().SetFloat("velocity", 0);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Alert() {
        while (true) {
            if (Vector3.Distance(transform.position, _player.position) < _playerDetectionDistance) {
                StopCoroutine(Patrol());
                StartCoroutine(Attack());
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Attack() {
        while (true) {
            if (Vector3.Distance(transform.position, _player.position) < _playerDetectionDistance) {
                StartCoroutine(Patrol());
                StartCoroutine(Alert());
                break;
            }
            if (Vector3.Distance(transform.position, _player.position) < _playerAttackDistance) {
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                GetComponent<Animator>().SetBool("attack", true);
                yield return new WaitForSeconds(3);
            } else {
                GetComponent<NavMeshAgent>().SetDestination(_player.position);
                GetComponent<Animator>().SetBool("attack", false);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartPatrol() => StartPatrol();

    public void StartAlert() => StartCoroutine(Alert());
}