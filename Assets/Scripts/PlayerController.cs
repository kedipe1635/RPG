using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] float movementSpeed;
    float currentSpeed;
    Rigidbody rb;
    Vector3 direction;
    [SerializeField] float shiftSpeed;
    [SerializeField] float jumpForce;
    bool isGrounded = true;
    [SerializeField] Animator anim;
    float stamina = 5f;
    [SerializeField] GameObject pistol, rifle, miniGun;
    bool isPistol, isRifle, isMiniGun;
    [SerializeField] Image pistolUI, rifleUI, miniGunUI, cusror;
    private int health;

    // AudioSource'a bir referans
    [SerializeField] AudioSource characterSounds;
    // Zýplama ses klibine bir referans
    [SerializeField] AudioClip jump;

    public bool dead;

    public enum Weapons
    {
        None,
        Pistol,
        Rifle,
        MiniGun
    }
    Weapons weapons = Weapons.None;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = movementSpeed;
        anim = GetComponent<Animator>();
        health = 100;
        // Eðer karakter oyuncuya ait deðilse
        if (!photonView.IsMine)
        {
            // Kamerayý oyuncunun Hierarchysinde bulma ve devre dýþý býrakma
            transform.Find("Main Camera").gameObject.SetActive(false);
            transform.Find("Canvas").gameObject.SetActive(false);
            // PlayerController kodunu devre dýþý býrakma
            this.enabled = false;
        }
    }
    


    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = movementSpeed;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            stamina += Time.deltaTime;
            currentSpeed = movementSpeed;
        }
        if (stamina > 5f)
        {
            stamina = 5f;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }



        if (direction.x != 0 || direction.z != 0)
        {

            anim.SetBool("Run", true);
            // Eðer herhangi bir ses oynamýyorsa ve yerdeysek...
            if (!characterSounds.isPlaying && isGrounded)
            {
                // Sesi oynatma kýsmý
                characterSounds.Play();
            }

        }

        if (direction.x == 0 && direction.z == 0)

        {

            anim.SetBool("Run", false);
            characterSounds.Stop();

        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);

            characterSounds.Stop();

            AudioSource.PlayClipAtPoint(jump, transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && isPistol)
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isRifle)
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.Rifle);
        }
        if (Input.GetKey(KeyCode.Alpha3) && isMiniGun)
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.MiniGun);
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.None);
        }

    }




    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }
    [PunRPC]
    public void ChooseWeapon(Weapons weapons)
    {
        anim.SetBool("Pistol", weapons == Weapons.Pistol);
        anim.SetBool("Assault", weapons == Weapons.Rifle);
        anim.SetBool("MiniGun", weapons == Weapons.MiniGun);
        anim.SetBool("NoWeapon", weapons == Weapons.None);
        pistol.SetActive(weapons == Weapons.Pistol);
        rifle.SetActive(weapons == Weapons.Rifle);
        miniGun.SetActive(weapons == Weapons.MiniGun);
        if (weapons != Weapons.None)
        {
            cusror.enabled = true;
        }
        else
        {
            cusror.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "pistol":
                if (!isPistol)
                {
                    isPistol = true;
                    pistolUI.color = Color.white;
                    ChooseWeapon(Weapons.Pistol);
                }
                break;
            case "rifle":
                if (!isRifle)
                {
                    isRifle = true;
                    rifleUI.color = Color.white;
                    ChooseWeapon(Weapons.Rifle);
                }
                break;
            case "minigun":
                if (!isMiniGun)
                {
                    isMiniGun = true;
                    miniGunUI.color = Color.white;
                    ChooseWeapon(Weapons.MiniGun);
                }
                break;
            default:
                break;
        }
        Destroy(other.gameObject);
    }
    
    public void ChangeHealth(int count)
    {
        // caný eksiltme kýsmý
        health -= count;
        // eðer can sýfýr veya altýna düþerse
        if (health <= 0)
        {
            dead = true;
            anim.SetBool("Die", true);
            ChooseWeapon(Weapons.None);
            this.enabled = false;
        }


    }
}