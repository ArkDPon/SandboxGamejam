using UnityEngine;

public enum Status { idle, walking, crouching, sprinting } //статусы игрока (стоит, ходит, бежит, ходит на приседе)

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]

public class PlayerController : MonoBehaviour
{
    public Status status;
    public LayerMask collisionLayer;
    public float crouchHeight = 1f;
    public PlayerInfo info;

    PlayerMovement movement;
    PlayerInput playerInput;
    AnimateCameraLevel animateCamLevel;
    
    float crouchCamAdjust;


    public void ChangeStatus(Status s)
    {
        if (status == s) return;
        status = s;
    }




    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        movement = GetComponent<PlayerMovement>();


        if (GetComponentInChildren<AnimateCameraLevel>())
        {
            animateCamLevel = GetComponentInChildren<AnimateCameraLevel>();
        }

        info = new PlayerInfo(movement.controller.radius, movement.controller.height);
        crouchCamAdjust = (crouchHeight - info.height) / 2f;
    }

    void Update()
    {
        UpdateMovingStatus();
        CheckCrouching();
        UpdateCamLevel();
    }


    void UpdateMovingStatus() //обновить статус игрока
    {

        if ((int)status <= 1 || isSprinting())
        {
            if (playerInput.input.magnitude > 0.02f)
            {
                ChangeStatus((shouldSprint()) ? Status.sprinting : Status.walking);
            }
            else
            {
                ChangeStatus(Status.idle);
            }
        }
    }

    public bool shouldSprint()
    {
        bool sprinting = false;
        sprinting = (playerInput.run && playerInput.input.y > 0);
        return sprinting;
    }


    void UpdateCamLevel() //обновить положение камеры
    {
        if (animateCamLevel == null) return;

        float level = 0f;
        if(status == Status.crouching)
        {
            level = crouchCamAdjust;
        }
        animateCamLevel.UpdateLevel(level);
    }

    void FixedUpdate()
    {
        DefaultMovement();
    }

    void DefaultMovement()
    {
        if (isSprinting() && isCrouching())
            Uncrouch();

        movement.Move(playerInput.input, isSprinting(), isCrouching());
        if (movement.grounded && playerInput.Jump())
        {
            if (status == Status.crouching)
            {
                if (!Uncrouch()) 
                    return;
            }

            movement.Jump(Vector3.up, 1f);
            playerInput.ResetJump();
        }
    }

    public bool isSprinting()
    {
        return (status == Status.sprinting && movement.grounded);
    }

    public bool isWalking()
    {
        if (status == Status.walking || status == Status.crouching)
        {
            return (movement.controller.velocity.magnitude > 0f && movement.grounded);
        }
        else
        {
            return false;
        }
    }
    public bool isCrouching()
    {
        return (status == Status.crouching);
    }
   
    void CheckCrouching()
    {
        if (!movement.grounded || (int)status > 2) return;

        if (playerInput.run)
        {
            Uncrouch();
            return;
        }

        if (playerInput.crouch)
        {
            if (status == Status.crouching)
            {
                Uncrouch();
            }
            else
            {
                Crouch(true);
            }
        }
    }

    public void Crouch(bool setStatus) //присесть
    {
        movement.controller.height = crouchHeight;
        if(setStatus) ChangeStatus(Status.crouching);
    }

    public bool Uncrouch() //встать
    {
        Vector3 bottom = transform.position - (Vector3.up * ((crouchHeight / 2) - info.radius));
        bool isBlocked = Physics.SphereCast(bottom, info.radius, Vector3.up, out var hit, info.height - info.radius, collisionLayer);
        if (isBlocked) return false; //если над игроком объект, ничего не произойдет
        movement.controller.height = info.height;
        ChangeStatus(Status.walking);
        return true;
    }

}

public class PlayerInfo //общая информация об игроке
{
    public float rayDistance;
    public float radius;
    public float height;
    public float halfradius;
    public float halfheight;

    public PlayerInfo(float r, float h)
    {
        radius = r; height = h;
        halfradius = r / 2f; halfheight = h / 2f;
        rayDistance =  halfheight + radius + .175f;
    }
}