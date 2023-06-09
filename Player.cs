using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 5f;
    PlayerController controller;
    Camera viewCamera;

    private void Start() {
        controller = GetComponent<PlayerController> ();
        viewCamera = Camera.main;
    }
    private void Update() {
        
        Vector3 moveInput = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move (moveVelocity);

        Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
            // Debug.DrawLine(ray.origin, point, Color.red);
        }
    }
}