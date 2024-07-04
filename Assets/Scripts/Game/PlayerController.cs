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

        _rb.velocity = new Vector3(moveHorizontal * _speed, _rb.velocity.y, moveVertical * _speed);

        if (Input.GetButtonDown("Jump") && _isGrounded) {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
}
