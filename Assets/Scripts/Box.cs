using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] Sprite fixedSprite;

    public Direction direction;
    public bool isFixed = false;
    public enum Direction { Left, Right, Up, Down };

    BoxCollider2D myBoxCollider;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myBoxCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        GetSpeed();
    }

    private void GetSpeed()
    {
        speed = FindObjectOfType<GameSession>().boxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBox();
        CheckFix();
    }

    private void CheckFix()
    {
        if (isFixed)
        {
            spriteRenderer.sprite = fixedSprite;
        }
    }

    private void MoveBox()
    {
        gameObject.transform.position = new Vector2(
                    gameObject.transform.position.x - speed * Time.deltaTime,
                    gameObject.transform.position.y);
    }


}
