using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Enemy : MonoBehaviourPunCallbacks
{
    protected GameObject[] players;

    [SerializeField] protected int health;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    protected GameObject player;
    protected Animator anim;
    protected Rigidbody rb;
    protected float distance;
    protected float timer;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CheckPlayers();
    }

    // Update is called once per frame
    private void Update()
    {
        //Uzaklýðý depolayacak bir deðiþken tanýtýyoruz
        //Mathf.Infinity - pozitif sonsuz
        float closestDistance = Mathf.Infinity;
        //Oyuncu listesinin her öðresi için tek tek çalýþacak kod
        foreach (GameObject closestPlayer in players)
        {
            //Düþman ve oyuncu arasýndaki uzaklýðýn hesaplanmasý
            float checkDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
            //Eðer bu oyuncuya olan mesafe bir önceki oyuncuya olan mesafeden daha az ise
            if (checkDistance < closestDistance)
            {
                //Önceki oyuncu hayattaysa
                if (closestPlayer.GetComponent<PlayerController>().dead == false)
                {
                    //Mevcut oyuncuyu en yakýn oyuncu olarak kaydetme 
                    player = closestPlayer;
                    //closestDistance deðerini bu oyuncuya olan uzaklýk olarak deðiþtirme
                    closestDistance = checkDistance;
                }
            }
        }
        //player deðiþkeninin içinde bir oyuncu olup olmadýðýný kontrol etme
        //Bu kontrol hatalarý önlememize yardýmcý olacaktýr
        if (player != null)
        {
            //Kodun geri kalaný deðiþmedi
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (!dead)
            {
                Attack();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!dead && player !=null)
        {
            Move();
        }
    }
    public virtual void Move()
    {
    }
    public virtual void Attack()
    {
    }
    [PunRPC]
    public void ChangeHealth(int count)
    {
        // can eksiltme kýsmý
        health -= count;
        // eðer can sýfýr veya altýna inerse...
        if (health <= 0)
        {
            // dead deðiþkeninin deðerini deðiþtirelim yani artýk Attack ve Move fonksiyonlarý çalýþmayacak
            dead = true;
            // düþmanýn colliderýný deaktive edelim
            GetComponent<Collider>().enabled = false;
            anim.enabled = true;
            // ölüm animasyonunu aktive edelim
            anim.SetBool("Die", true);
        }
    }
    void CheckPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Invoke("CheckPlayers", 3f);
    }
    public void GetDamage(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }




}