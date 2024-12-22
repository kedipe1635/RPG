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
        //Uzakl��� depolayacak bir de�i�ken tan�t�yoruz
        //Mathf.Infinity - pozitif sonsuz
        float closestDistance = Mathf.Infinity;
        //Oyuncu listesinin her ��resi i�in tek tek �al��acak kod
        foreach (GameObject closestPlayer in players)
        {
            //D��man ve oyuncu aras�ndaki uzakl���n hesaplanmas�
            float checkDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
            //E�er bu oyuncuya olan mesafe bir �nceki oyuncuya olan mesafeden daha az ise
            if (checkDistance < closestDistance)
            {
                //�nceki oyuncu hayattaysa
                if (closestPlayer.GetComponent<PlayerController>().dead == false)
                {
                    //Mevcut oyuncuyu en yak�n oyuncu olarak kaydetme 
                    player = closestPlayer;
                    //closestDistance de�erini bu oyuncuya olan uzakl�k olarak de�i�tirme
                    closestDistance = checkDistance;
                }
            }
        }
        //player de�i�keninin i�inde bir oyuncu olup olmad���n� kontrol etme
        //Bu kontrol hatalar� �nlememize yard�mc� olacakt�r
        if (player != null)
        {
            //Kodun geri kalan� de�i�medi
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
        // can eksiltme k�sm�
        health -= count;
        // e�er can s�f�r veya alt�na inerse...
        if (health <= 0)
        {
            // dead de�i�keninin de�erini de�i�tirelim yani art�k Attack ve Move fonksiyonlar� �al��mayacak
            dead = true;
            // d��man�n collider�n� deaktive edelim
            GetComponent<Collider>().enabled = false;
            anim.enabled = true;
            // �l�m animasyonunu aktive edelim
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