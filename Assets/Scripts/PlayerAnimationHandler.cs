using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DIPProject
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator; 
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Horizontal") == 0 &&
                Input.GetAxis("Vertical") == 0)
            {
                animator.SetTrigger("Stop");
                animator.ResetTrigger("Left");
                animator.ResetTrigger("Right");
                animator.ResetTrigger("Down");
                animator.ResetTrigger("Up");
                animator.ResetTrigger("Active");
            }
            else
            {
                animator.ResetTrigger("Stop");
                animator.SetTrigger("Active");
                if (Input.GetAxis("Horizontal") < 0) animator.SetTrigger("Left");
                else if (Input.GetAxis("Horizontal") > 0) animator.SetTrigger("Right");

                if (Input.GetAxis("Vertical") < 0) animator.SetTrigger("Down");
                else if (Input.GetAxis("Vertical") > 0) animator.SetTrigger("Up");
            }
        }
    }
}