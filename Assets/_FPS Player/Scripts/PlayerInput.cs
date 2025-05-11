using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 input
    {
        get
        {
            Vector2 i = Vector2.zero;
            i.x = Input.GetAxis("Horizontal");
            i.y = Input.GetAxis("Vertical");
            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }

    public bool run
    {
        get { return Input.GetKey(KeyCode.LeftShift); }
    }

    public bool crouch
    {
        get { return Input.GetKeyDown(KeyCode.C); }
    }

    int jumpCooldown;
    bool jump;

    private void Start()
    {
        jumpCooldown = -1;
    }


    public void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            jump = false;
            jumpCooldown++;
        }
        else if (jumpCooldown > 0)
            jump = true;
    }

    public bool Jump()
    {
        return jump;
    }

    public void ResetJump()
    {
        jumpCooldown = -1;
    }
}
