using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Abilities/Offensive/Projectile")]
public class Projectile : OffensiveAbility
{
    public Vector3 point;
    public GameObject projectilePrefab;

    public override void UseAbility()
    {
        GameObject go = Instantiate(projectilePrefab, playerAbilities.transform.position + Vector3.forward, Quaternion.identity);

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        AbilityDamager ad = projectilePrefab.GetComponent<AbilityDamager>();
		ad.teamWhoShot = teamWhoCast;

        if (plane.Raycast(ray, out distance))
        {
            point = ray.GetPoint(distance);
            go.transform.LookAt(point + (Vector3.up / 2));
        }
    }

}
