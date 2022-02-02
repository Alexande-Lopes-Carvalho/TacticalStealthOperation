using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int _health)
    {
        slider.maxValue = _health;
        slider.value = _health;
    }
    
    public void SetCurrentHealth(int _health)
    {
        slider.value = _health;
    }
}
