using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject robot;
    [SerializeField] AudioClip clipHit;
    [SerializeField] AudioClip clipMiss;
    [SerializeField] AudioClip clipMistake;

    AudioSource audioSource;
    Animator animator;
    BoxCollider2D targetCollider;
    GameObject latestBox;
    int combo = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = robot.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        targetCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        // If arrow inputs
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            #region Hit animation
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HitLeft();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                HitRight();
            }
            #endregion

            if (targetCollider.IsTouchingLayers(LayerMask.GetMask("Box")))
            {
                var boxDirection = latestBox.GetComponent<Box>().direction;

                if (boxDirection == Box.Direction.Up && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    OnSuccessfulHit();
                }
                else if (boxDirection == Box.Direction.Down && Input.GetKeyDown(KeyCode.DownArrow))
                {
                    OnSuccessfulHit();
                }
                else if (boxDirection == Box.Direction.Left && Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnSuccessfulHit();
                }
                else if (boxDirection == Box.Direction.Right && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnSuccessfulHit();
                }
                else
                {
                    audioSource.PlayOneShot(clipMistake, 0.5f);
                    TakeDamage();
                }
            }
            else
            {
                audioSource.PlayOneShot(clipMistake, 0.5f);
                TakeDamage();
            }
        }
    }

    private void TakeDamage()
    {
        FindObjectOfType<GameSession>().TakeHealth();
        combo = 0;
    }

    private void HitRight()
    {
        animator.SetTrigger("HitRight");
    }

    private void HitLeft()
    {
        animator.SetTrigger("HitLeft");
    }

    private void OnSuccessfulHit()
    {
        audioSource.PlayOneShot(clipHit, 0.2f);
        FindObjectOfType<GameSession>().AddToScore(1);
        latestBox.GetComponent<Box>().isFixed = true;
        IncreaseCombo();
    }

    private void IncreaseCombo()
    {
        if (combo >= 4)
        {
            combo = 0;
            FindObjectOfType<GameSession>().GiveHealth();
        }
        else
        {
            combo++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        latestBox = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (latestBox.GetComponent<Box>().isFixed == false)
        {
            audioSource.PlayOneShot(clipMiss, 0.5f);
            TakeDamage();
        }
    }
}
