using UnityEngine;

public class ClopTrigger : MonoBehaviour
{
    public MainLoop main;

    void OnMouseDown(){
        main.GoClope();
    }
}
