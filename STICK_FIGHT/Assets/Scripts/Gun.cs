using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public GameObject bulletPrefab;
    public Transform enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = (1 << LayerMask.NameToLayer("EnemyBone")) + (1 << LayerMask.NameToLayer("Ignore Raycast"));
        layerMask = ~layerMask;
        if (Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, layerMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, layerMask);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point);
        }
    }

    public void Shoot()
    {
        if (enemy.rotation.eulerAngles.y == 0)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles.x, firePoint.rotation.eulerAngles.y + 180, 180 - firePoint.rotation.eulerAngles.z));
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
