
using UnityEngine;

public class DoorPush : MonoBehaviour, Interactable
{
    public Animator anim;

    public AudioSource openSound;
    public AudioSource closeSound;
    
    public void Interact()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        {
            anim.SetBool("opened", !anim.GetBool("opened"));
            if(closeSound == null)
            {
                PlaySoundStatic.InstSound(openSound, gameObject);
            }
            else
            {
                if(anim.GetBool("opened") == false)
                {
                    PlaySoundStatic.InstSound(closeSound, gameObject);
                }
                else
                {
                    PlaySoundStatic.InstSound(openSound, gameObject);
                }
            }
           // inst.AddComponent<AudioSourceOptimizer>();
        }
    }
   void Start()
    {
        anim.ForceStateNormalizedTime(1f);
    }
}
