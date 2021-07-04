using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public GameObject camera;

    public float cameraClimbSpeed = 2f;

    public Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool up = true;

    private void Awake() {
        Debug.Log("Y: " + camera.transform.position.y);
    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(x != 0 || z != 0) {
            HeadMovement();
            animator.SetBool("isMoving", true);
        }
        else {
            animator.SetBool("isMoving", false);
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HeadMovement() {
        float value = Mathf.Sin(Time.deltaTime * cameraClimbSpeed);

        if(up) {
            camera.transform.position += new Vector3(0, value, 0);

            if(camera.transform.position.y >= 3.8f) {
                camera.transform.position = new Vector3(camera.transform.position.x, 3.8f, camera.transform.position.z);
                up = false;
            }
                
        }
        else {
            camera.transform.position -= new Vector3(0, value, 0);

            if(camera.transform.position.y <= 3.4f) {
                camera.transform.position = new Vector3(camera.transform.position.x, 3.4f, camera.transform.position.z);
                up = true;
            }
                
        }
    }
}
