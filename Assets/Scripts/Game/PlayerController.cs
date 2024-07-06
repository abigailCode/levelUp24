using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] float _speed = 5f;
    [SerializeField] float _jumpForce = 5f;
    Rigidbody _rb;
    bool _isGrounded;

    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (!GameManager.Instance.isActive) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        // Make the player look at the direction it's moving
        if (moveDirection != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
        }

        // Mover el objeto en la dirección de movimiento
        transform.Translate(moveDirection * _speed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && _isGrounded) {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) _isGrounded = true;
    }

    public float GetSpeed() => _speed;

    public void SetSpeed(float speed) => _speed = speed;

    public float GetJumpForce() => _jumpForce;

    public void SetJumpForce(float jumpForce) => _jumpForce = jumpForce;

    public void SetAttributes(float speed, float jumpForce) {
        _speed = speed;
        _jumpForce = jumpForce;
    }

    void OnDestroy() {
        if (name == "Player") GameManager.Instance.GameOver();
    }
}
