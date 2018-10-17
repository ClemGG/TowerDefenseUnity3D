using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour {

    [SerializeField] private bool usedForPool = false;
    /*Pour que la FixedUpdate m'évite des ralentissements, il faudrait vérifier que l'ensemble des spawners ne vident pas complètement
     la piscine d'objets avant la fin du timer du prefab, et donc vérifier que le timer du spawner ait une valeur assez élevé pour que le rythme
     d'instantiation soit suffisament long pour ne pas vider la piscine avant que l'objet ne se désactive lui-même.
     Apparemment, 0.3f pour le spawner c'est amplement suffisant pour un timer de 5f pour l'objet (Ca sera ma référence).
     C'est juste pour éviter que les objets ne restent trop longtemps en scène si la piscine ne s'est pas encore complètement vidée.
     Mais bon, normalement pour les spawners c'est des ennemis, du loot, etc... Donc c'est le joueur qui enclenche 
     la désactivation et pas un timer, et on attend que tous les ennemis/objets disparaissent pour les réinstancier... Bref, en tout cas ça marche,
     faudra juste faire gaffe à ça.*/

    private float timer = 0;
    [SerializeField] private float delay;




    // Use this for initialization
    void Start () {
        if(!usedForPool)
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (usedForPool)
        {
            timer += Time.deltaTime;

            if (timer > delay)
            {
                timer = 0;
                gameObject.SetActive(false);
            }

        }
    }
}
