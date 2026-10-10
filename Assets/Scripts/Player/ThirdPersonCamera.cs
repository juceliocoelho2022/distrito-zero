using UnityEngine;

namespace DistritoZero.Player
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;

        [Header("Orbit")]
        [SerializeField] private float distance = 7f;
        [SerializeField] private float targetHeight = 1.5f;
        [SerializeField] private float mouseSensitivity = 2.5f;
        [SerializeField] private float minPitch = -70f;
        [SerializeField] private float maxPitch = 70f;
        [SerializeField] private float smooth = 12f;

        private float yaw;
        private float pitch = 15f;

        private void Start()
        {
            if (target != null) yaw = target.eulerAngles.y;
            LockCursor();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }

            if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButtonDown(0))
            {
                LockCursor();
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
                pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                pitch = ClampPitch(pitch, minPitch, maxPitch);
            }

            Vector3 pivot = target.position + Vector3.up * targetHeight;
            Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = pivot + orbitRotation * Vector3.back * distance;

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                Time.deltaTime * smooth
            );
            transform.rotation = Quaternion.LookRotation(pivot - transform.position, Vector3.up);
        }

        public static float ClampPitch(float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        private static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
