using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Pistol
{
    void Start()
    {
        //At��lar aras�ndaki gecikme (istedi�iniz de�eri ayarlayabilirsiniz)
        cooldown = 0.2f;
        //Bu silah tam otomatik olarak ate� eder; fare d��mesini tuttu�umuz s�rece ate� etmeye devam edecektir
        // (endi�elenmeyin: yukar�da tan�mlad���n�z gecikme hesaba kat�lacakt�r!)
        auto = true;
        ammoBackPack = 90;
        ammoCurrent = 30;
        ammoMax = 30;

    }




}