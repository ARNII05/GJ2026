using UnityEngine;

public class NearbyObjects : MonoBehaviour
{
    public Box actualBox;
    public Truck truck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Box":
                actualBox = collision.GetComponentInParent<Box>();
                break;

            case "Truck":
                truck = collision.GetComponentInParent<Truck>();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Box":
                actualBox = null;
                break;

            case "Truck":
                truck = null;
                break;
        }
    } 
}
