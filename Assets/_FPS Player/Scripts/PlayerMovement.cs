using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    public float antiBumpFactor = .75f;

    public Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;

    public bool grounded = false;
    public Vector3 jump = Vector3.zero;
    Vector3 jumpedDir;

    public Vector3 floorOffset;

    bool forceGravity;
    float forceTime = 0;
    float jumpPower;


    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void FixedUpdate()
    {
        if (forceTime > 0)
        {
            if(forceGravity)
                moveDirection.y -= gravity * Time.deltaTime;
            grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        }
    }


    public void Move(Vector2 input, bool sprint, bool crouching)
    {
        if(forceTime > 0)
            return;

        float speed = (!sprint) ? walkSpeed : runSpeed;
        if (crouching) speed = crouchSpeed;

        if (grounded)
        {
            moveDirection = new Vector3(input.x, -antiBumpFactor, input.y);
            moveDirection = transform.TransformDirection(moveDirection) * speed;
            UpdateJump();
        }
        else
        {
            Vector3 adjust = new Vector3(input.x, 0, input.y);
            adjust = transform.TransformDirection(adjust);
            jumpedDir += adjust * Time.fixedDeltaTime * jumpPower * 2f;
            jumpedDir = Vector3.ClampMagnitude(jumpedDir, jumpPower);
            moveDirection.x = jumpedDir.x;
            moveDirection.z = jumpedDir.z;
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0; 
    }

    public void Move(Vector3 direction, float speed, float appliedGravity)
    {
        if (forceTime > 0)
            return;

        Vector3 move = direction * speed;
        if (appliedGravity > 0)
        {
            moveDirection.x = move.x;
            moveDirection.y -= gravity * Time.deltaTime * appliedGravity;
            moveDirection.z = move.z;
        }
        else
            moveDirection = move;

        UpdateJump();

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    public void Move(Vector3 direction, float speed, float appliedGravity, float setY)
    {
        if (forceTime > 0)
            return;

        Vector3 move = direction * speed;
        if (appliedGravity > 0)
        {
            moveDirection.x = move.x;
            if (setY != 0) moveDirection.y = setY * speed;
            moveDirection.y -= gravity * Time.deltaTime * appliedGravity;
            moveDirection.z = move.z;
        }
        else
            moveDirection = move;

        UpdateJump();

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    public void Jump(Vector3 dir, float mult)
    {
        jump = dir * mult;
    }

    public void UpdateJump()
    {
        if (jump != Vector3.zero)
        {
            Vector3 dir = (jump * jumpSpeed);
            if (dir.x != 0) moveDirection.x = dir.x;
            if (dir.y != 0) moveDirection.y = dir.y;
            if (dir.z != 0) moveDirection.z = dir.z;

            Vector3 move = moveDirection;
            jumpedDir = move; move.y = 0;
            jumpPower = Mathf.Min(move.magnitude, jumpSpeed);
            jumpPower = Mathf.Max(jumpPower, walkSpeed);
        }
        else
            jumpedDir = Vector3.zero;
        jump = Vector3.zero;
    }

    public void ForceMove(Vector3 direction, float speed, float time, bool applyGravity)
    {
        forceTime = time;
        forceGravity = applyGravity;
        moveDirection = direction * speed;
    }
}
