using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SyncController : MonoBehaviour {
    [SerializeField] GameObject[] _balls;
    PlayerController _controller;
    EnemyController _enemyController;
    NavMeshAgent _navMeshAgent;
    GroupController _groupController;

    void Start() {
        _controller = GetComponent<PlayerController>();
        _enemyController = GetComponent<EnemyController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _groupController = GetComponent<GroupController>();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            HandlePlayerCollision(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Enemy")) {
            HandleEnemyCollision(collision.gameObject);
        }
    }

    void HandlePlayerCollision(GameObject player) {
        GroupController playerGroup = player.GetComponent<GroupController>();

        if (_controller.enabled) {
            // Already a player
            return;
        }

        // Convert this enemy to player
        _controller.enabled = true;
        _controller.SetAttributes(player.GetComponent<PlayerController>().GetSpeed(), player.GetComponent<PlayerController>().GetJumpForce());
        _enemyController.enabled = false;
        _navMeshAgent.enabled = false;
        ChangeBalls(0);

        gameObject.tag = "Player";

        // Add this enemy to the player's group
        playerGroup.AddToGroup(gameObject);
        _groupController.groupMembers = new List<GameObject>(playerGroup.groupMembers); // Sync group members

        // Update GameManager
        GameManager.Instance.ChangePlayerCount();
    }

    void HandleEnemyCollision(GameObject enemy) {
        GroupController enemyGroup = enemy.GetComponent<GroupController>();

        // Convert enemy group to players
        foreach (GameObject member in new List<GameObject>(enemyGroup.groupMembers)) {
            member.GetComponent<SyncController>().HandlePlayerCollision(gameObject);
        }

        // TODO: Fix this

        //// Merge groups if both are enemies
        // if (_groupController.groupMembers[0].CompareTag("Enemy") && enemy.CompareTag("Enemy")) {
        //    foreach (GameObject member in enemyGroup.groupMembers) {
        //        _groupController.AddToGroup(member);
        //    }
        //    enemyGroup.groupMembers.Clear();

        //    // Set the first member as the leader
        //    SetGroupLeader(_groupController.groupMembers[0]);
        // } else {
        //    // Compare group sizes
        //    if (_groupController.GetGroupSize() >= enemyGroup.GetGroupSize()) {
        //        // Convert enemy group to players
        //        foreach (GameObject member in enemyGroup.groupMembers) {
        //            member.GetComponent<SyncController>().HandlePlayerCollision(gameObject);
        //        }
        //    } else {
        //        // Convert player group to enemies
        //        foreach (GameObject member in _groupController.groupMembers) {
        //            member.GetComponent<SyncController>().HandleEnemyConversion();
        //        }
        //        GameManager.Instance.GameOver();
        //    }
        // }
    }

    public void HandlePlayerConversion() {
        if (_controller.enabled) {
            _controller.enabled = false;
            _enemyController.enabled = true;
            _navMeshAgent.enabled = true;
            ChangeBalls(0);
            gameObject.tag = "Enemy";

            // Add this object to its own group
            _groupController.groupMembers.Clear();
            _groupController.groupMembers.Add(gameObject);

            // Set the first member as the leader
            SetGroupLeader(_groupController.groupMembers[0]);

            // Update GameManager
            GameManager.Instance.ChangePlayerCount();
        }
    }

    public void HandleEnemyConversion() {
        if (_enemyController.enabled) {
            _controller.enabled = true;
            _enemyController.enabled = false;
            _navMeshAgent.enabled = false;
            ChangeBalls(1);
            gameObject.tag = "Player";

            // Add this object to its own group
            _groupController.groupMembers.Clear();

            GroupController playerGroupController = GameObject.Find("Player").GetComponent<GroupController>();
            playerGroupController.AddToGroup(gameObject);
            _groupController.groupMembers = playerGroupController.groupMembers;

            // Update GameManager
            GameManager.Instance.ChangePlayerCount();
        }
    }

    void SetGroupLeader(GameObject leader) {
        foreach (GameObject member in _groupController.groupMembers) {
            EnemyController enemyController = member.GetComponent<EnemyController>();
            if (member == leader) {
                enemyController.SetLeader(true);
            } else {
                enemyController.SetLeader(false);
            }
        }
    }

    void ChangeBalls(int i) {
        foreach (GameObject ball in _balls) ball.SetActive(i == 0);
    }
}