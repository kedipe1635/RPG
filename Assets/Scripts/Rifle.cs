using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Pistol
{
    void Start()
    {
        //Atýþlar arasýndaki gecikme (istediðiniz deðeri ayarlayabilirsiniz)
        cooldown = 0.2f;
        //Bu silah tam otomatik olarak ateþ eder; fare düðmesini tuttuðumuz sürece ateþ etmeye devam edecektir
        // (endiþelenmeyin: yukarýda tanýmladýðýnýz gecikme hesaba katýlacaktýr!)
        auto = true;
        ammoBackPack = 90;
        ammoCurrent = 30;
        ammoMax = 30;

    }




}