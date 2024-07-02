using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveDirection;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        // Get the input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement direction
        moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate() {
        // Apply the movement
        Vector3 velocity = moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
    }
}
