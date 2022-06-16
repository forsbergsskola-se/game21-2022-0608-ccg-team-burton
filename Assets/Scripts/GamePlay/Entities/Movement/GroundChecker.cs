using UnityEngine;

namespace GamePlay.Entities.Movement
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded;
        [SerializeField] private float _groundCheckLength = 0.53f;
        [SerializeField] private float _groundCheckRadius = 0.53f;
        
        public bool IsRoofed;
        [SerializeField] private float _roofCheckLength = 0.53f;
        [SerializeField] private float _roofCheckRadius = 0.53f;
        [SerializeField] private LayerMask _groundLayers;

        private void Update()
        {
            CheckIfGrounded();
            CheckIfRoofed();
        }
        
        private void CheckIfRoofed()
        {
            var pos = transform.position;
            var ray = new Vector2(pos.x, pos.y);

            IsRoofed = Physics2D.CircleCast(ray, _roofCheckRadius, Vector2.up, _roofCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.up * _roofCheckLength, Color.red);
        }
        

        
        private void CheckIfGrounded()
        {
            var pos = transform.position;
            var ray = new Vector2(pos.x, pos.y);

            IsGrounded = Physics2D.CircleCast(ray, _groundCheckRadius, Vector2.down, _groundCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.down * _groundCheckLength, Color.magenta);
        }

        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position + Vector3.down * _groundCheckLength, _groundCheckRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + Vector3.up * _roofCheckLength, _roofCheckRadius);
        }
    }
}