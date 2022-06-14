using UnityEngine;

namespace GamePlay.Entities.Movement
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded;
        [SerializeField] private float _groundCheckLength = 1f;
        [SerializeField] private float _groundCheckRadius = 0.5f;
        [SerializeField] private LayerMask _groundLayers;

        private void Update()
        {
            CheckIfGrounded();
        }

        
        private void CheckIfGrounded()
        {
            var pos = transform.position;
            var ray = new Ray(pos, Vector3.down);
                
            IsGrounded = Physics.SphereCast(ray, _groundCheckRadius, _groundCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.down * _groundCheckLength, Color.magenta);
        }

        
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + Vector3.down * _groundCheckLength, _groundCheckRadius);
        }
    }
}
