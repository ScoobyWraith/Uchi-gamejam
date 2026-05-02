using UnityEngine;

public class ModalView : MonoBehaviour
{
    public SliderCarousel Carousel;
    
    public void CloseModal()
    {
        gameObject.SetActive(false);
    }

    public void OpenModal()
    {
        gameObject.SetActive(true);
        Carousel.Open();
    }
}
