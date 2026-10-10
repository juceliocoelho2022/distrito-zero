using UnityEngine;

namespace DistritoZero.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float jumpHeight = 1.4f;
        [SerializeField] private float gravity = -22f;
        private CharacterController controller;
        private float verticalVelocity;

        private void Awake() => controller = GetComponent<CharacterController>();

        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 move = new Vector3(x, 0f, z).normalized;
            var cam = Camera.main;
            if (cam != null && move.sqrMagnitude > 0.01f)
            {
                Vector3 forward = cam.transform.forward; forward.y = 0f; forward.Normalize();
                Vector3 right = cam.transform.right; right.y = 0f; right.Normalize();
                move = (forward * z + right * x).normalized;
                transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * 12f);
            }

            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
            if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            verticalVelocity += gravity * Time.deltaTime;
            move.y = verticalVelocity;
            controller.Move(move * (speed * Time.deltaTime));
        }
    }
}
