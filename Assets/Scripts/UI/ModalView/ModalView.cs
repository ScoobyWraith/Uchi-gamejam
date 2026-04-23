using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalView : MonoBehaviour
{
    public void CloseModal()
    {
        gameObject.SetActive(false);
    }
}
