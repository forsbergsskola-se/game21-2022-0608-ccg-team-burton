using UnityEngine;

namespace GamePlay.Entities.Movement
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("VERTICAL SIDES")]
        public bool IsGrounded;
        [SerializeField] private float _groundCheckLength = 0.53f;
        [SerializeField] private float _groundCheckRadius = 0.53f;
        
        public bool IsRoofed;
        [SerializeField] private float _roofCheckLength = 0.53f;
        [SerializeField] private float _roofCheckRadius = 0.53f;
        [SerializeField] private LayerMask _groundLayers;

        [Header("HORIZONTAL SIDES")]
        private bool LeftWallHit;
        [SerializeField] private float _leftCheckLength = 0.53f;
        [SerializeField] private float _leftCheckRadius = 0.53f;
        
        private bool RightWallHit;
        [SerializeField] private float _rightCheckLength = 0.53f;
        [SerializeField] private float _rightCheckRadius = 0.53f;
        
        private Vector3 pos;
        private Vector2 ray;

        private void Update()
        {
            pos = transform.position;
            ray = new Vector2(pos.x, pos.y);
            CheckIfGrounded();
            CheckIfRoofed(pos, ray);
        }
        
        private void CheckIfRoofed(Vector3 pos, Vector2 ray)
        {
            // check above for collisions
            IsRoofed = Physics2D.CircleCast(ray, _roofCheckRadius, Vector2.up, _roofCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.up * _roofCheckLength, Color.white );
        }
        

        
        private void CheckIfGrounded()
        {
           // check below for collisions
            IsGrounded = Physics2D.CircleCast(ray, _groundCheckRadius, Vector2.down, _groundCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.down * _groundCheckLength, Color.black);
        }
        
        
        
        private void CheckLeftWall()
        {
            LeftWallHit = Physics2D.CircleCast(ray, _leftCheckRadius, Vector2.left, _leftCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.down * _leftCheckLength, Color.cyan);
        }
        
        
        
        private void CheckRightWall()
        {
            RightWallHit = Physics2D.CircleCast(ray, _rightCheckRadius, Vector2.right, _rightCheckLength, _groundLayers);
            Debug.DrawRay(pos, Vector3.down * _rightCheckLength, Color.red);
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black; // colour of ground check gizmo
            Gizmos.DrawSphere(transform.position + Vector3.down * _groundCheckLength, _groundCheckRadius);
            
            Gizmos.color = Color.white; // colour of roof check gizmo
            Gizmos.DrawSphere(transform.position + Vector3.up * _roofCheckLength, _roofCheckRadius);
            
            
            Gizmos.color = Color.red; // colour of left wall check gizmo
            Gizmos.DrawSphere(transform.position + Vector3.left * _leftCheckLength, _leftCheckRadius);
            
            Gizmos.color = Color.cyan; // colour of right wall check gizmo
            Gizmos.DrawSphere(transform.position + Vector3.right * _rightCheckLength, _rightCheckRadius);
        }
    }
}