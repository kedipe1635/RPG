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
        // E�er d��man ve oyuncu aras�ndaki uzakl�k b�ce�in fark etme menzilinde ise:
        // VE d��man ve oyuncu aras�ndaki uzakl�k d��man�n sald�r� menzilinden uzak ise:
        if (distance < detectionDistance && distance > attackDistance)
        {
            // D��man oyuncuya d�necek
            transform.LookAt(player.transform);
            // Ko�ma animasyonunu aktifle�tirelim
            anim.SetBool("Run", true);
            // B�ce�i ilerletelim
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        // E�er ko�ullar do�ru de�ilse:
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
        // Zamanlay�c� aktifle�tirelim
        timer += Time.deltaTime;
        // E�er oyuncu ile d��man aras�ndaki mesafe sald�r� menzili i�indeyse ve zamanlay�c� de�eri sald�r� bekleme s�resinden b�y�kse
        if (distance < attackDistance && timer > cooldown)
        {
            // Zamanlay�c�y� s�f�rlayal�m
            timer = 0;
            // Oyuncu kodunu getirip can eksiltme fonksiyonunu �al��t�ral�m
            player.GetComponent<PlayerController>().ChangeHealth(damage);
            // Sald�r� animasyonunu aktifle�tirelim
            anim.SetBool("Attack", true);
        }
        // E�er ko�ullar do�ru de�ilse...
        else
        {
            // Sald�r� animasyonunu deaktive edelim
            anim.SetBool("Attack", false);
        }
    }
}