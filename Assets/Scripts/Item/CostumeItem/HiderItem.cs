using System;
using UnityEngine;

public class HiderItem : SC_InteractableItem
{
    public override void Interact()
    {
        //Le dice al player que se ponga en su posicion
        //Le dice que si se mueve se desesconde
        //Le dice al enemigo que no puede ver al player
    }

    private void OnTriggerExit(Collider other)
    {
        //Le dice al enemigo que puede ver al jugador
    }
}
