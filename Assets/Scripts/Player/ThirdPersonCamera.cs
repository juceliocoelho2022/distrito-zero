using UnityEngine;

namespace DistritoZero.Player
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 4.5f, -7f);
        [SerializeField] private float smooth = 8f;

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + target.TransformDirection(offset);
            transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * smooth);
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }
}
