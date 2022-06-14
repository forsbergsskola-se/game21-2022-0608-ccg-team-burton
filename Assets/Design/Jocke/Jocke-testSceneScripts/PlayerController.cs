using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float gravity = -20;
    [SerializeField] Transform groundCheck;
    public LayerMask groundLayer;
    private Vector3 direction;

    void Update()
    {

        float hinput = Input.GetAxis("Horizontal");
        direction.x = hinput * speed;
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        controller.Move(direction * Time.deltaTime);

        if (isGrounded)
        {

            if (Input.GetButtonDown("Jump"))
          {
             direction.y = jumpHeight;
          }

        }
        direction.y += gravity * Time.deltaTime;
        
    }
}
