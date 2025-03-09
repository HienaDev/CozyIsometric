using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSlide : MonoBehaviour
{

    [SerializeField] private GameObject slide;

    public void Toggleslide() => slide.SetActive(!slide.activeSelf);
}
