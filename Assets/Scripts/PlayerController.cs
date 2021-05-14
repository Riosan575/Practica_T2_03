using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    private Rigidbody2D rb2d;
    private bool puedoSaltar = false;
    private bool estoyenlaescalera = false;
    public GameObject kunairight;
    public GameObject kunaileft;
    private float subir = 10f;
    private bool estoymuerto = false;
    private bool muero = false;
    //PUNTAJE
    public Text PuntajeText;
    public Text VidaText;

    private int puntajeScore = 0;
    private int scoreVida = 3; 

    //intangible
    private float maxItangibleTime = 1f;
    private float intangibleTime = 0f;
    private bool esIntangible = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PuntajeText.text = "Puntaje: " + puntajeScore;
        VidaText.text = "VIDAS: " + scoreVida;


        SetIdleAnimation();
        if(Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false;
            SetRunAnimation();
            rb2d.velocity = new Vector2(10, rb2d.velocity.y);
        }
        else
        {
            SetIdleAnimation();
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            SetRunAnimation();
            rb2d.velocity = new Vector2(-10, rb2d.velocity.y);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            SetSlideAnimation();
        }
        if(Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            var jump = 80f;
            SetJumpAnimation();
            rb2d.velocity = Vector2.up * jump;
        }
        if(estoyenlaescalera == true)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {            
                SetClimpAnimation();    
                rb2d.velocity = new Vector2(rb2d.velocity.x, subir);
                Debug.Log("ESTOY AQUI");
            }
            if(Input.GetKey(KeyCode.DownArrow))
            { 
                SetClimpAnimation();               
                rb2d.velocity = new Vector2(rb2d.velocity.x, -subir);
                Debug.Log("ESTOY AQUI");
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {            
            if(!sr.flipX)
            {
                SetTrowAnimation();
                var position = new Vector2(transform.position.x + 3f, transform.position.y - 0.1f);
                Instantiate(kunairight,position,kunairight.transform.rotation);
            }
            else
            {
                SetTrowAnimation();
                var position = new Vector2(transform.position.x - 1.3f, transform.position.y - 0.1f);
                Instantiate(kunaileft,position,kunaileft.transform.rotation);
            }
        }
        if(estoymuerto){
            SetDeadAnimation();
            HabilitarColisionConEnemigo();
                intangibleTime = 0;
                esIntangible = false;
                sr.enabled = true;
             rb2d.velocity = new Vector2(0, rb2d.velocity.y);

        }
        if (esIntangible && intangibleTime < maxItangibleTime)
        {
            Debug.Log("Intangible");
            intangibleTime += Time.deltaTime;
            Parpadear();
            DeshabilitarColisionConEnemigo();
        }

        if (intangibleTime >= maxItangibleTime)
        {
            HabilitarColisionConEnemigo();
            intangibleTime = 0;
            esIntangible = false;
            sr.enabled = true;
        }           
        if(Input.GetKey(KeyCode.R))
        {                
            rb2d.velocity = new Vector2(rb2d.velocity.x,-1); 
            SetGlideAnimation();
            rb2d.gravityScale = 0.5f;
        } 
        CaidaMuere();   
        if(muero)
        {
            SetDeadAnimation();
            HabilitarColisionConEnemigo();
                intangibleTime = 0;
                esIntangible = false;
                sr.enabled = true;
             rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }        
    }
    private void HabilitarColisionConEnemigo()
    {
        Physics2D.IgnoreLayerCollision(3, 6, false);
    }
    private void DeshabilitarColisionConEnemigo()
    {
        Physics2D.IgnoreLayerCollision(3, 6, true);
    }
    private void Parpadear()
    {
        sr.enabled = !sr.enabled;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        puedoSaltar = true;
        if(other.gameObject.tag == "Enemy")
        {            
            DisminuirVida();
            if(scoreVida == 0){
                Debug.Log(scoreVida);                
                estoymuerto = true;                
            } 
            esIntangible = true;                   
        }
        if(other.gameObject.tag == "Piso")
        {
            rb2d.gravityScale = 40f;
        }
    }
    void OnTriggerStay2D(Collider2D other)    
    {
        if(other.gameObject.tag == "escalera")
        {
            estoyenlaescalera = true;
            rb2d.gravityScale = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }         
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "escalera")
        {
            estoyenlaescalera = false;
            rb2d.gravityScale = 40;
        }
    }
    private void SetIdleAnimation()
    {
        animator.SetInteger("Estado",0);
    }
    private void SetRunAnimation()
    {
        animator.SetInteger("Estado",1);
    }
    private void SetJumpAnimation()
    {
        animator.SetInteger("Estado",2);
    }
    private void SetClimpAnimation()
    {
        animator.SetInteger("Estado",3);
    }
    private void SetGlideAnimation()
    {
        animator.SetInteger("Estado",4);
    }
    private void SetSlideAnimation()
    {
        animator.SetInteger("Estado",5);
    }
    private void SetTrowAnimation()
    {
        animator.SetInteger("Estado",6);
    }
    private void SetDeadAnimation()
    {
        animator.SetInteger("Estado",7);
    }
    public void incremetarPuntajeEnemy()
    {
        puntajeScore += 10;
    }
    public void DisminuirVida()
    {        
        scoreVida -= 1;
    }
    private void CaidaMuere()
    {
        Debug.Log("Mi velocidad es: " + rb2d.velocity.y);
        if (rb2d.velocity.y < -100)
        {
            muero = true;
            scoreVida = 0;
            VidaText.text = "VIDAS: " + scoreVida;
        }
    }
}
