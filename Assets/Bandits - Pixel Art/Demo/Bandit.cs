using UnityEngine;
using System.Collections;

public class Bandit : Character
{

    [SerializeField] float m_jumpForce = 7.5f;



    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    private void Update()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            animator.SetBool("Grounded", m_grounded);
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            animator.SetBool("Grounded", m_grounded);
        }

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        m_body2d.velocity = new Vector2(inputX * moveSpeed, m_body2d.velocity.y);

        animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        else if (Input.GetKeyDown("space") && m_grounded)
        {
            animator.SetTrigger("Jump");
            m_grounded = false;
            animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            animator.SetInteger("AnimState", 2);

        else if (m_combatIdle)
            animator.SetInteger("AnimState", 1);

        else
            animator.SetInteger("AnimState", 0);
    }




}
