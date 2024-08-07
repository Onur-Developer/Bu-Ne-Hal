using UnityEngine;

namespace Scipts.CameracChanger
{
    public class CameraChanger : MonoBehaviour
    {
        [SerializeField] private float xAxis;
        [SerializeField] private float yAxis;
        [SerializeField] private bool isCollide;


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                GameManager.instance.SetCollide(false);
                GameManager.instance.SetScreenAxis(xAxis,yAxis);
                GameManager.instance.SetCollide(isCollide);
            }
        }
    }
}
