using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{
    [SerializeField] float speed;
    [SerializeField] float detectionDistance;
    float patrolTimer;

    public override void Move()
    {
        // Eðer düþman ve oyuncu arasýndaki uzaklýk böceðin fark etme menzilinde ise:
        // VE düþman ve oyuncu arasýndaki uzaklýk düþmanýn saldýrý menzilinden uzak ise:
        if (distance < detectionDistance && distance > attackDistance)
        {
            // Düþman oyuncuya dönecek
            transform.LookAt(player.transform);
            // Koþma animasyonunu aktifleþtirelim
            anim.SetBool("Run", true);
            // Böceði ilerletelim
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        // Eðer koþullar doðru deðilse:
        else if (distance > detectionDistance)
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            patrolTimer += Time.deltaTime;
            anim.SetBool("Run", true);
            if (patrolTimer > 10)
            {
                transform.Rotate(new Vector3(0, 90, 0));
                patrolTimer = 0;
            }
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }


    public override void Attack()
    {
        // Zamanlayýcý aktifleþtirelim
        timer += Time.deltaTime;
        // Eðer oyuncu ile düþman arasýndaki mesafe saldýrý menzili içindeyse ve zamanlayýcý deðeri saldýrý bekleme süresinden büyükse
        if (distance < attackDistance && timer > cooldown)
        {
            // Zamanlayýcýyý sýfýrlayalým
            timer = 0;
            // Oyuncu kodunu getirip can eksiltme fonksiyonunu çalýþtýralým
            player.GetComponent<PlayerController>().ChangeHealth(damage);
            // Saldýrý animasyonunu aktifleþtirelim
            anim.SetBool("Attack", true);
        }
        // Eðer koþullar doðru deðilse...
        else
        {
            // Saldýrý animasyonunu deaktive edelim
            anim.SetBool("Attack", false);
        }
    }
}