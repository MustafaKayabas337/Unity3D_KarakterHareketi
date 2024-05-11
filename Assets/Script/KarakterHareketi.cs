using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterHareketi : MonoBehaviour
{
    public float karakterHizi;
    public float donusHizi;
    public float ziplamaHizi;

    private CharacterController characterController;
    private float ziplamaYuksekligi; 
    private float varsayilanStepOffset;

    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        varsayilanStepOffset = characterController.stepOffset;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float yatayInput = Input.GetAxis("Horizontal");
        float dikeyInput = Input.GetAxis("Vertical");

        Vector3 hareket = new Vector3(yatayInput, 0, dikeyInput);
        float buyukluk = Mathf.Clamp01(hareket.magnitude);
        hareket.Normalize();

        ziplamaYuksekligi += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            animator.SetBool("isJumping", false);
            characterController.stepOffset = varsayilanStepOffset;
            ziplamaYuksekligi = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                ziplamaYuksekligi = ziplamaHizi;
                animator.SetBool("isJumping", true);
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        bool isRunning = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
            isRunning = true;
        }
        else
        {
            animator.SetBool("isRunning", false);
            isRunning = false;
        }

        Vector3 karakterHareketi = hareket * karakterHizi * buyukluk;
        if (isRunning) karakterHareketi *= 2f;
        karakterHareketi.y = ziplamaYuksekligi;

        characterController.Move(karakterHareketi * Time.deltaTime);

        if(hareket != Vector3.zero)
        {
            Quaternion hedefRotasyon = Quaternion.LookRotation(hareket, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, hedefRotasyon, donusHizi * Time.deltaTime);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

    }
}
