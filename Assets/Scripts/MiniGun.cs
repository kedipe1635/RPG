using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGun : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        //At��lar aras�ndaki gecikme (istedi�iniz de�eri ayarlayabilirsiniz)
        cooldown = 0.1f;
        //Bu silah tam otomatik olarak ate� eder; fare d��mesini tuttu�umuz s�rece ate� etmeye devam edecektir (endi�elenmeyin: yukar�da tan�mlad���n�z gecikme hesaba kat�lacakt�r!
        auto = true;
        ammoBackPack = 1000;
        ammoCurrent = 100;
        ammoMax = 100;
    }


    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 drift = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition + drift);
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