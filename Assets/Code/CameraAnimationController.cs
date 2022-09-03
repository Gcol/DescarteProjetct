using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    public Animator animator;
    public string currentState;
    public string nextState;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (nextState != null)
        {
            if (ReadyNextAnimation() == true)
            {
                ChangeAnimation(nextState);
            }
        }
    }

    public void ChangeAnimation(string newState)
    {
        if (currentState == newState) return;

        StartCoroutine(ExampleCoroutine(newState));


    }


    IEnumerator ExampleCoroutine(string newState)
    {
        if (ReadyNextAnimation() == false)
        {
            nextState = newState;
        }
        else
        {
            nextState = null;
            animator.Play(newState);
        }

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

    }

    public bool ReadyNextAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
            return true;
        }
        return false;
    }
}
