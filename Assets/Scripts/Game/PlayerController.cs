using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] float _speed = 5f;
    [SerializeField] float _jumpForce = 5f;
    Rigidbody _rb;
    private bool _isGrounded;
    Vector3 _moveDirection;

    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (!GameManager.instance.isActive) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (Input.GetButtonDown("Jump") && _isGrounded) {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    void FixedUpdate() {
        if (!GameManager.instance.isActive) return;

        Vector3 velocity = _moveDirection * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + velocity);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) _isGrounded = true;
    }
}
