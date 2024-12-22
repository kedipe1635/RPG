using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private void Start()
    {
        auto = false;
        cooldown = 0;
        ammoBackPack = 30;
        ammoCurrent = 10;
        ammoMax = 10;
    }


    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition);
        RaycastHit hit;

        //Bu kod RaycastHit hit sat�r�ndan sonra yaz�lmal�d�r
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if (hit.collider.CompareTag("enemy"))
            {
                // 10 say�s�n� istedi�iz �ekilde de�i�tirebilirsiniz bir merminin verece�i zarar� belirtiyor
                hit.collider.gameObject.GetComponent<Enemy>().GetDamage(10);
            }
            Destroy(gameBullet, 1);
        }
    }
}