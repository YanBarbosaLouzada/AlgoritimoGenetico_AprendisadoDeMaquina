using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta é a classe que define o comportamento de um inimigo no jogo.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class BaseEnemy : MonoBehaviour {

    public DNA dna;  // Aqui guardamos informações sobre a "genética" do inimigo.
    public BaseEnemy mainBody;  // Este é o corpo principal do inimigo.
    public float maxGeneNumber = 360;  // É o valor máximo para "genes".
    public int geneQuantity = 3;  // Quantos "genes" temos.

    // Aqui estamos criando o "cérebro" do inimigo, que toma decisões.
    [System.Serializable]
    public class Brain
    {
        public float rayRange = 1;  // Isso diz o quão longe ele pode ver.
        public float timeAlive = 0;  // Isso conta quanto tempo ele viveu.
        public bool _Spike;  // Ele usa isso para saber se encontrou espinhos.
        public bool _Danger;  // Ele usa isso para saber se encontrou perigo.
        public bool isAlive;  // Isso diz se ele ainda está vivo.

        // Este método faz o inimigo se mover com base no que ele vê.
        public void ExecuteMovement(BaseEnemy baseEnemy)
        {   
            baseEnemy.FireRaycasts();  // Ele "olha" ao redor.
            baseEnemy.MoveTwo();  // Ele se move com base no que "vê".
        }
    }

    private bool _Spike;  // Variáveis para guardar se ele encontrou espinhos.
    private bool _Danger;  // Variáveis para guardar se ele encontrou perigo.

    // Este método inicia o inimigo quando ele é criado.
    public void Initialize(BaseEnemy be)
    {
        mainBody = be;
        dna = new DNA((int)maxGeneNumber, geneQuantity);  // Cria sua "genética".
        Physics2D.queriesStartInColliders = false;

        // Às vezes, ele faz uma "mutação" genética para ser diferente.
        if (Random.Range(0, 100) < 11)
            dna.Mutate();
    }

    // Aqui ele "dispara" raios para ver o que está ao seu redor.
    void FireRaycasts()
    {
        _Spike = false;
        _Danger = false;

        // Ele olha em diferentes direções.
        Vector2 diagonalRight = (mainBody.transform.up + mainBody.transform.right) / 2;
        Vector2 diagonalLeft = (mainBody.transform.up - mainBody.transform.right) / 2;

        // Ele desenha linhas vermelhas para mostrar o que ele "vê".
        Debug.DrawRay(mainBody.transform.position, diagonalRight * brain.rayRange, Color.red);
        Debug.DrawRay(mainBody.transform.position, diagonalLeft * brain.rayRange, Color.red);
        Debug.DrawRay(mainBody.transform.position, mainBody.transform.up * brain.rayRange, Color.red);    
        
        // Ele dispara raios e vê se eles tocam em algo.
        RaycastHit2D hit = Physics2D.Raycast(mainBody.transform.position, diagonalRight, brain.rayRange);
        if (hit)
            CompareTags(hit);
        hit = Physics2D.Raycast(mainBody.transform.position, diagonalLeft, brain.rayRange);
        if (hit)
            CompareTags(hit);
        hit = Physics2D.Raycast(mainBody.transform.position, mainBody.transform.up, brain.rayRange);
        if (hit)
            CompareTags(hit);
    }

    // Ele verifica o que foi tocado pelos raios e guarda se são espinhos ou perigo.
    public void CompareTags(RaycastHit2D hit)
    {
        if (hit.transform.CompareTag("Spike"))
            _Spike = true;
        else if (hit.transform.CompareTag("Danger"))
            _Danger = true;
    }

    // Ele move de uma maneira específica com base no que "vê".
    void MoveTwo()
    {
        float direction = 0;
        if (_Spike)
        {
            direction = dna.GetGene(0);
            // Às vezes ele para de se mover se virar muito.
            if (dna.GetGene(2) > 180)
                mainBody.rb.velocity = Vector2.zero;
        }
        else if (_Danger)
        {
            direction = dna.GetGene(1);
            // Às vezes ele para de se mover se virar muito.
            if (dna.GetGene(2) > 180)
                mainBody.rb.velocity = Vector2.zero;
        }

        // Ele muda a direção em que está olhando e se move para frente.
        mainBody.transform.rotation *= Quaternion.Euler(0, 0, direction);
        mainBody.rb.AddForce(mainBody.transform.up.normalized * mainBody.speed * Time.deltaTime, ForceMode2D.Force);
    }

    public Brain brain;  // Aqui temos o "cérebro" do inimigo.
    public bool useBrain = true;  // Isso diz se ele deve usar o "cérebro" para se mover.
    public float maxVelocityMagnitude = 5;  // A velocidade máxima que ele pode alcançar.
    public float speed = 60;  // Quão rápido ele se move.
    public bool quickBounce = true;  // Se ele deve "pular" quando bate em algo.
    public bool randomizeBounceDirection = true;  // Se a direção do "pulo" deve ser aleatória.
    public bool limitVelocity = true;  // Se a velocidade deve ser limitada.
    private Rigidbody2D rb;  // Componente que permite ele se mover.

    // Este método é chamado quando o inimigo é criado.
    public virtual void Start() {
        rb = GetComponent<Rigidbody2D>();  // Ele pega o componente que permite ele se mover.
        Physics2D.queriesStartInColliders = false;
        Initialize(this);  // Ele inicia o inimigo.
    }

    // Este método é chamado a cada quadro do jogo.
    void Update() {
        if (useBrain)
            brain.ExecuteMovement(this);  // Ele decide como se mover com base no "cérebro".
        else
            Move();  // Ele se move de uma maneira simples.

        if (limitVelocity)
            LimitVelocity();  // Ele se certifica de não se mover muito rápido.

        // Se ele bater em algo perigoso, ele vira à direita.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1);
        if (hit)
            if (hit.collider.CompareTag("Danger"))
                TurnRight();
    }

    // Limita a velocidade do inimigo para não ser muito rápido.
    protected void LimitVelocity() {
        if (Mathf.Abs(rb.velocity.x) > maxVelocityMagnitude)
            rb.velocity = new Vector2(maxVelocityMagnitude * Mathf.Sign(rb.velocity.x), rb.velocity.y);

        if (Mathf.Abs(rb.velocity.y) > maxVelocityMagnitude)
            rb.velocity = new Vector2(rb.velocity.x, maxVelocityMagnitude * Mathf.Sign(rb.velocity.y));
    }

    // Este método é chamado quando o inimigo colide com algo.
    protected virtual void Move() {
        rb.AddForce(transform.up.normalized * speed * Time.deltaTime, ForceMode2D.Force);
    }

    // Este método é chamado quando o inimigo colide com algo perigoso ou espinhos.
    protected virtual void OnCollisionEnter2D(Collision2D collision) 
    {
        transform.up = -collision.contacts[0].point;

        if(collision.transform.CompareTag("Spike"))
            DestroyEnemy();  // Se ele bater em espinhos, ele é destruído.
        else if(collision.transform.CompareTag("Danger"))
            DestroyEnemy();  // Se ele bater em algo perigoso, ele é destruído.

        if (quickBounce)
            rb.AddForce(collision.relativeVelocity, ForceMode2D.Impulse);  // Ele dá um "pulo" se bater em algo.

        if (randomizeBounceDirection)
            transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-60f, 60f));  // A direção do "pulo" é aleatória.
    }

    // Este método é chamado para destruir o inimigo.
    public virtual void DestroyEnemy() {
        gameObject.SetActive(false);  // Ele é desativado no jogo.
    }

    // Este método faz o inimigo virar à direita.
    public void TurnRight() {
        rb.velocity = Vector2.zero;
        transform.rotation *= Quaternion.Euler(0, 0, -90);  // Ele vira 90 graus à direita.
    }
}
