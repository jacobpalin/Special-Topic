using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UMACharacterEditor : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    public Transform sliders;

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
        foreach (Transform child in sliders.transform)
        {
            child.gameObject.GetComponent<Slider>().onValueChanged.AddListener(OnSliderChanged);
        }
    }

    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
    }

    void Updated(UMAData data)
    {
        dna = avatar.GetDNA();
        foreach (Transform child in sliders.transform)
        {
            child.gameObject.GetComponent<Slider>().value = dna[child.gameObject.name].Get();
        }
    }

    public void OnSliderChanged(float val)
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go == null)
        {
            return;
        }
        Slider mySlider = go.GetComponent(typeof(Slider)) as Slider;
        if (mySlider != null)
        {
            Debug.Log("Slider name:" + go.name + ", value: " + val);
            dna[EventSystem.current.currentSelectedGameObject.name].Set(val);
            avatar.BuildCharacter();
        }
    }
}