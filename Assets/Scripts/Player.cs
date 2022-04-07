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
        float hor = Input.GetAxisRaw("Horizontal");         // ���� �Է� �ޱ�.
        SetVelocityX(hor * 5f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetVelocityY(0f);
            rigid2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }

    private void SetVelocityX(float x)
    {
        Vector2 velocity = rigid2D.velocity;                // ���� �ӵ�.
        velocity.x = x;                                     // x�� �ӵ��� ����.
        rigid2D.velocity = velocity;                        // �� �ӵ��� ����.
    }
    private void SetVelocityY(float y)
    {
        Vector2 velocity = rigid2D.velocity;                // ���� �ӵ�.
        velocity.y = y * 5f;                                // x�� �ӵ��� ����.
        rigid2D.velocity = velocity;                        // �� �ӵ��� ����.
    }
}
