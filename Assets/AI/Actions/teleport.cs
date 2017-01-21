using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;


[RAINAction]
public class teleport : RAINAction
{
    // ai and player gameobjects
    private GameObject self = null;
    private GameObject player = null;
    private float teleportDistance = 25;
    private float teleportComponent;
    
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        //get the player and ai game objects from memory
        self = ai.WorkingMemory.GetItem<GameObject>("self");
        player = ai.WorkingMemory.GetItem<GameObject>("player");
        teleportComponent = teleportDistance / Mathf.Sqrt(2);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        //random number for pseudo-random teleport
        int xRandom = Random.Range(0, 2);
        int zRandom = Random.Range(0, 2);

        //get player's position and add some values to it to get the spot to be teleported to
        Vector3 teleportspot = player.transform.position;
        teleportspot.x += teleportComponent;
        teleportspot.z += teleportComponent;
        if (Random.value <= 0.5) teleportspot.x -= 2 * teleportComponent;
        if (Random.value <= 0.5) teleportspot.z -= 2 * teleportComponent;

        //if (xRandom == 0) {
        //    teleportspot.x = teleportspot.x - teleportComponent;
        //} else {
        //    teleportspot.x = teleportspot.x + teleportComponent;
        //}

        //if (zRandom == 0) {
        //    teleportspot.z = teleportspot.z - teleportComponent;
        //} else {
        //    teleportspot.z = teleportspot.z + teleportComponent;
        //}

        teleportspot.y = teleportspot.y + 5;

        //teleport monster
        self.transform.position = teleportspot;
        

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}