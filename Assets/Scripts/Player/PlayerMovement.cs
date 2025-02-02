﻿using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f; // The speed at which the player will move.
    Vector3 movement; // The vector stores the direction of the player's movement.
    Animator anim; // Reference to the animator component.
    Rigidbody playerRigidbody; // Reference to the player's rigid body.
    int floorMask; // A layer mask so that a ray can be cast at game objects on the floor layer.
    float camRayLength = 100f; // The length of the ray from the camera into the scene.
    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        // Move the player around the scene.
        Move(h, v);
        // Turn the player to face the mouse cursor.
        Turning();
        // Animate the player.
        Animating(h, v);
    }
    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);
        // Normalize the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;
        // Move the player to its current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }
    void Turning()
    {
        
        // Create a ray from the mouse cursor on the screen in the camera’s direction.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;
        // Perform the raycast, and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor of the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;
            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;
            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
            
        }
    }
    void Animating(float h, float v)
    {
        // Create a true Boolean if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;
        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }
}