﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPlacer : MonoBehaviour
{
    public List<Vector3> ClickPoints = new List<Vector3>();
    public float SphereCastRadius = 1.0f;
    public int PlacementMode = 0;
    GameObject mouseCube;
    RoadNetwork roadNetwork;
    bool start = true;
    GameObject currentPoint;
    GameObject previousPoint;

    void Start()
    {
        mouseCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mouseCube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        roadNetwork = GameObject.Find("RoadNetwork").GetComponent<RoadNetwork>();
    }

	void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray, SphereCastRadius);
        Vector3 worldHitPosition = Vector3.zero;
        bool hitSuccess = false;
        if (hits.Length > 0 && GUIUtility.hotControl == 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.tag == "GameWorld")
                {
                    RaycastHit preciseGroundCollision;
                    if (hit.collider.Raycast(ray, out preciseGroundCollision, Mathf.Infinity))
                    {
                        Vector3 precisePosition = preciseGroundCollision.point;
                        mouseCube.transform.position = precisePosition;
                        worldHitPosition = precisePosition;
                        hitSuccess = true;
                    }
                }
                else if (hit.collider.gameObject.tag == "RoadNode")
                {
                    Debug.Log("Hitting road node");
                    Vector3 hitPosition = hit.collider.gameObject.transform.position;
                    mouseCube.transform.position = hitPosition;
                    worldHitPosition = hitPosition;
                    hitSuccess = true;
                    break;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && hitSuccess)
        {
            //ClickPoints.Add(worldHit.point);
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = worldHit.point;
            //cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Debug.Log("Creating point...");
            currentPoint = roadNetwork.CreateNode(worldHitPosition);

            if (!start)
            {
                currentPoint.GetComponent<RoadNode>().AddConnection(previousPoint);
                start = true;
            }
            else
            {
                start = false;
            }

            previousPoint = currentPoint;
        }
	}

    public void SetMode(string mode)
    {
        Debug.Log(mode);
        PlacementMode += 1;
    }
}
