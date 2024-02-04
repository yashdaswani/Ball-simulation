using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] bool isSystem;
    [SerializeField] ParticleSystemForceField defaultfield;
    [SerializeField] ParticleSystemForceField specialfield;
    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;
    [SerializeField] float minwidth;
    [SerializeField] float maxwidth;
    bool isDefaultNow;
    Vector3 targetPos;
    [SerializeField] float moveSpeed;
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // Check if the ray hits something
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 collisionPoint = hit.point;
                        if (!isSystem)
                        {
                            Instantiate(prefab, collisionPoint, prefab.transform.rotation);
                        }
                        else
                        {
                            targetPos = collisionPoint;
                            if (!isDefaultNow)
                            {
                                isDefaultNow = true;
                                prefab.transform.GetChild(0).GetComponent<ParticleSystem>().externalForces.RemoveAllInfluences();
                                prefab.transform.GetChild(0).GetComponent<ParticleSystem>().externalForces.AddInfluence(specialfield);
                                ps1.externalForces.RemoveAllInfluences();
                                ps2.externalForces.RemoveAllInfluences();
                                ps1.externalForces.AddInfluence(specialfield);
                                ps2.externalForces.AddInfluence(specialfield);
                                var tr1 = ps1.trails;
                                var tr2 = ps2.trails;
                                tr1.lifetime = new ParticleSystem.MinMaxCurve(maxwidth);
                                tr2.lifetime = new ParticleSystem.MinMaxCurve(maxwidth);
                            }
                            var trailModule = prefab.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                            trailModule.lifetime = new ParticleSystem.MinMaxCurve(maxwidth);
                        }
                    }
                }
            }
        }
        else
        {
            if(isDefaultNow)
            {
                isDefaultNow = false;
                prefab.transform.GetChild(0).GetComponent<ParticleSystem>().externalForces.RemoveAllInfluences();
                prefab.transform.GetChild(0).GetComponent<ParticleSystem>().externalForces.AddInfluence(defaultfield);
                ps1.externalForces.RemoveAllInfluences();
                ps2.externalForces.RemoveAllInfluences();
                ps1.externalForces.AddInfluence(defaultfield);
                ps2.externalForces.AddInfluence(defaultfield);
                var tr1 = ps1.trails;
                var tr2 = ps2.trails;
                tr1.lifetime = new ParticleSystem.MinMaxCurve(minwidth);
                tr2.lifetime = new ParticleSystem.MinMaxCurve(minwidth);
            }
            var trailModule = prefab.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
            trailModule.lifetime = new ParticleSystem.MinMaxCurve(minwidth);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void LateUpdate()
    {
        if(isSystem)
        {
            prefab.transform.position = Vector3.Lerp(prefab.transform.position, targetPos, Time.deltaTime * moveSpeed);
            ps1.transform.position = Vector3.Lerp(ps1.transform.position, targetPos, Time.deltaTime * moveSpeed);
            ps2.transform.position = Vector3.Lerp(ps2.transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }
}
