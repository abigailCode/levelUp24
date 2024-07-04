using UnityEngine;
using UnityEngine.AI;

public class SyncController : MonoBehaviour {
    private PlayerController _controller;
    private EnemyController _enemyController;
    private NavMeshAgent _navMeshAgent;
    private Renderer _renderer;
    private GroupController _groupController;

    void Start() {
        _controller = GetComponent<PlayerController>();
        _enemyController = GetComponent<EnemyController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _renderer = GetComponent<Renderer>();
        _groupController = GetComponent<GroupController>();
        _groupController.groupMembers.Add(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player"))
            HandlePlayerCollision(collision.gameObject);
        else if (collision.gameObject.CompareTag("Enemy"))
            HandleEnemyCollision(collision.gameObject);
    }

    void HandlePlayerCollision(GameObject player) {
        GroupController playerGroup = player.GetComponent<GroupController>();

        if (_controller.enabled) return; // Already a player
        PlayerController playerController = player.GetComponent<PlayerController>();

        // Convert this enemy to player
        _controller.enabled = true;
        _controller.SetAttributes(playerController.GetSpeed(), playerController.GetJumpForce());
        _enemyController.enabled = false;
        _navMeshAgent.enabled = false;
        _renderer.material.color = Color.red;
        gameObject.tag = "Player";

        // Add this enemy to the player's group
        playerGroup.AddToGroup(gameObject);
        SyncGroups(playerGroup);
    }

    void HandleEnemyCollision(GameObject enemy) {
        GroupController enemyGroup = enemy.GetComponent<GroupController>();

        // Merge groups if both are enemies
        if (_groupController.groupMembers[0].CompareTag("Enemy") && enemy.CompareTag("Enemy")) {
            foreach (GameObject member in enemyGroup.groupMembers) {
                _groupController.AddToGroup(member);
            }
            SyncGroups(_groupController);
        } else {
            // Compare group sizes
            if (_groupController.GetGroupSize() >= enemyGroup.GetGroupSize()) {
                // Convert enemy group to players
                foreach (GameObject member in enemyGroup.groupMembers) {
                    member.GetComponent<SyncController>().HandlePlayerCollision(gameObject);
                }
            } else {
                // Convert player group to enemies
                foreach (GameObject member in _groupController.groupMembers) {
                    member.GetComponent<SyncController>().HandleEnemyConversion();
                }
                GameManager.Instance.GameOver();
            }
        }
    }

    public void HandleEnemyConversion() {
        if (_controller.enabled) {
            _controller.enabled = false;
            _enemyController.enabled = true;
            _navMeshAgent.enabled = true;
            _renderer.material.color = Color.blue;
            gameObject.tag = "Enemy";

            // Add this object to its own group
            _groupController.groupMembers.Clear();
            _groupController.groupMembers.Add(gameObject);

            // Set the first member as the leader
            SetGroupLeader(_groupController.groupMembers[0]);
        }
    }

    void SetGroupLeader(GameObject leader) {
        foreach (GameObject member in _groupController.groupMembers) {
            EnemyController enemyController = member.GetComponent<EnemyController>();
            enemyController.SetLeader(member == leader);
        }
    }

    void SyncGroups(GroupController group) {
        foreach (GameObject member in group.groupMembers) {
            SyncController syncController = member.GetComponent<SyncController>();
            if (syncController != this) {
                syncController._groupController.groupMembers = group.groupMembers;
            }
        }
    }
}
