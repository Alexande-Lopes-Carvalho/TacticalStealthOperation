using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextColor : MonoBehaviour
{

 
    private void OnMouseEnter()
    {
        gameObject.GetComponent<TMP_Text>().color = Color.red;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<TMP_Text>().color = Color.white;
    }
}
