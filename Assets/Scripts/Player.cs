using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    new SpriteRenderer renderer;
    Rigidbody2D rigid2D;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(Random.value, Random.value, Random.value, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");         // 수평 입력 받기.
        SetVelocityX(hor * 5f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetVelocityY(0f);
            rigid2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }

    private void SetVelocityX(float x)
    {
        Vector2 velocity = rigid2D.velocity;                // 현재 속도.
        velocity.x = x;                                     // x축 속도를 대입.
        rigid2D.velocity = velocity;                        // 원 속도에 대입.
    }
    private void SetVelocityY(float y)
    {
        Vector2 velocity = rigid2D.velocity;                // 현재 속도.
        velocity.y = y * 5f;                                // x축 속도를 대입.
        rigid2D.velocity = velocity;                        // 원 속도에 대입.
    }
}
