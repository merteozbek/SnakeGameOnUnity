using UnityEngine;
using System.Collections;
using System;

public class Snake : MonoBehaviour {
    
    private Snake next;
    static public Action<String> hit;
    public void Setnext(Snake IN)
    {
        next = IN;
    }

    public Snake GetNext()
    {
        return next;
    }
    public void RemoveTail()//Yılanın kuyrugunu silme işlemi
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other) // kendine carptıgında game over işlemini veriyoruz
    {
        if (hit != null)
        {
            hit(other.tag);
        }
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);
        }
    }
}
