using UnityEngine;

namespace GamePlay.Entities.Movement
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        [SerializeField] private float _groundCheckLength = 1f;
        [SerializeField] private float _groundCheckRadius = 0.5f;
        [SerializeField] private LayerMask _groundLayers;

        private void Update()
        {
            CheckIfGrounded();
        }

        private void CheckIfGrounded()
        {
            var ray = new Ray(transform.position, Vector3.down);
            IsGrounded = Physics.SphereCast(ray, _groundCheckRadius, _groundCheckLength, _groundLayers);
            // Debug.DrawRay(transform.position, Vector3.down * groundCheckLength, Color.magenta);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + Vector3.down * _groundCheckLength, _groundCheckRadius);
        }
    }
}
