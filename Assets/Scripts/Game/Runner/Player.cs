using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Столкнулись с игроком!");
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"Всё ещё сталкиваемся с {collision.gameObject.name}");
        // Например, наносим урон пока касается
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"Перестали сталкиваться с {collision.gameObject.name}");
    }
    
    // Триггер (без физического столкновения, просто пересечение)
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Объект {other.name} вошёл в зону");
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Объект {other.name} всё ещё в зоне");
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Объект {other.name} вышел из зоны");
    }
}
