using UnityEngine;

public class SC_WallChanger : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private GameObject[] walls;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var wall in walls)
            {
                wall.GetComponent<Renderer>().material = this.material;
            }
        }
    }
}
