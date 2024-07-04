using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    [SerializeField] float _playerDetectionDistance, _playerAttackDistance;
    private Vector3 _destination;
    private Transform _player;
    private List<GameObject> _groupMembers;
    private bool isLeader = false;

    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _groupMembers = new List<GameObject> { gameObject };
    }

    void Update() {
        if (!GameManager.Instance.isActive) StopAllCoroutines();
        if (!isLeader && _groupMembers.Count > 0) FollowLeader();
    }

    void SetRandomDestination() {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * 10f, out hit, 10f, NavMesh.AllAreas)) {
            _destination = hit.position;
            SetDestinationForGroup(_destination);
        }
    }

    void SetDestinationForGroup(Vector3 destination) {
        if (isLeader) {
            foreach (GameObject member in _groupMembers) {
                NavMeshAgent agent = member.GetComponent<NavMeshAgent>();
                if (agent != null) {
                    agent.SetDestination(destination);
                    //member.GetComponent<Animator>().SetFloat("velocity", 2);
                }
            }
        }
    }

    public void StartGroupPatrol(List<GameObject> groupMembers) {
        _groupMembers = groupMembers;
        if (isLeader) StartCoroutine(Patrol());
    }

    public void StartGroupAlert(List<GameObject> groupMembers) {
        _groupMembers = groupMembers;
        if (isLeader) StartCoroutine(Alert());
    }

    public void StartGroupCrazy(List<GameObject> groupMembers) {
        _groupMembers = groupMembers;
        if (isLeader) StartCoroutine(GoCrazy());
    }

    IEnumerator Patrol() {
        SetRandomDestination();
        while (true) {
            if (Vector3.Distance(transform.position, _destination) < 1.5f) {
                //foreach (GameObject member in _groupMembers)
                //    member.GetComponent<Animator>().SetFloat("velocity", 0);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                SetRandomDestination();
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
                SetDestinationForGroup(transform.position);
                foreach (GameObject member in _groupMembers) {
                    if (member.TryGetComponent<NavMeshAgent>(out var agent)) {
                        agent.velocity = Vector3.zero;
                        //member.GetComponent<Animator>().SetBool("attack", true);
                    }
                }
                yield return new WaitForSeconds(3);
            } else {
                SetDestinationForGroup(_player.position);
                //foreach (GameObject member in _groupMembers)
                //    member.GetComponent<Animator>().SetBool("attack", false);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator GoCrazy() {
        while (true) {
            if (Random.Range(0, 1) < 0.3f) GetComponent<SyncController>().HandleEnemyConversion();
            yield return new WaitForSeconds(3f);
        }
    }

    public void SetLeader(bool isLeader) {
        this.isLeader = isLeader;
        if (isLeader) StartGroupPatrol(_groupMembers);
    }

    void FollowLeader() {
        if (_groupMembers.Count > 0) {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            GameObject leader = _groupMembers[0];
            if (agent != null && leader != null) {
                agent.SetDestination(leader.transform.position);
                //GetComponent<Animator>().SetFloat("velocity", 2);
            }
        }
    }

    public List<GameObject> GetGroupMembers() => _groupMembers;
}