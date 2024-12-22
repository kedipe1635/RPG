using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGun : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        //Atýþlar arasýndaki gecikme (istediðiniz deðeri ayarlayabilirsiniz)
        cooldown = 0.1f;
        //Bu silah tam otomatik olarak ateþ eder; fare düðmesini tuttuðumuz sürece ateþ etmeye devam edecektir (endiþelenmeyin: yukarýda tanýmladýðýnýz gecikme hesaba katýlacaktýr!
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

        //Bu kod RaycastHit hit satýrýndan sonra yazýlmalýdýr
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if (hit.collider.CompareTag("enemy"))
            {
                // 10 sayýsýný istediðiz þekilde deðiþtirebilirsiniz bir merminin vereceði zararý belirtiyor
                hit.collider.gameObject.GetComponent<Enemy>().GetDamage(10);
            }
            Destroy(gameBullet, 1);
        }
    }
}