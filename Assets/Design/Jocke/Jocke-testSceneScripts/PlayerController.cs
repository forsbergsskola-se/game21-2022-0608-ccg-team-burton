using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 6f;

    private Vector3 direction;

    void Update()
    {
        float Hinput = Input.GetAxis("Horizontal");
        direction.x = Hinput * speed;
        controller.Move(direction * Time.deltaTime);
    }
}
