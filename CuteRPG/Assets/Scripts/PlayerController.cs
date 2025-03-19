using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 10f;
    public Animator animator;

    private void Update()
    {
        float move = Input.GetAxis("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed);
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        transform.Translate(0, 0, move * Time.deltaTime);
        transform.Rotate(0, rotation * Time.deltaTime, 0);

        animator.SetFloat("Speed", move);
    }
}