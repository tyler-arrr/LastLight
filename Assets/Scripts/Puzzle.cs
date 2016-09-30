﻿using UnityEngine;
using System.Collections;
using VolumetricLines.Utils;
namespace VolumetricLines
{
    public class Puzzle : MonoBehaviour
    {
        public int numMirrors;
        public GameObject[] mirrors;
        public GameObject[] lines;
        public GameObject[] lights;
        public GameObject[] volumetricLines;
        public Transform[][] linePositions;
        public GameObject[] keys;
        public int keyCheck = 0;
        //public VolumetricLineBehavior[] linesScripts;
        //public MeshRenderer[] mesh;

        Vector3 incidenceAngle;
        Vector3 reflectionAngle;
        public bool puzzleComplete = false;

        public int j = 0;
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            j = 0;
            /*
            mesh = new MeshRenderer[volumetricLines.Length];
            linesScripts = new VolumetricLineBehavior[volumetricLines.Length];
            for(int i = 0; i < volumetricLines.Length; i++)
            {
                linesScripts[i] = volumetricLines[i].GetComponent<VolumetricLineBehavior>();
                mesh[i] = volumetricLines[i].GetComponent<MeshRenderer>();
                linesScripts[i].enabled = false;
                mesh[i].enabled = false;
            }
            */
            for (int i = 0; i < lines.Length; i++)
            {
                LineRenderer tempLine = lines[i].GetComponent<LineRenderer>();
                tempLine.enabled = false;
            }
            for (int i = 0; i < lights.Length; i++)
            {
                RayTest(lights[i], i);
            }
            for(int i = 0; i < keys.Length; i++)
            {
                KeyScript key = keys[i].GetComponent<KeyScript>();
                if(key.unlock)
                {
                    keyCheck++;
                }
            }
            if(keyCheck == keys.Length)
            {
                puzzleComplete = true;
            }
            else
            {
                keyCheck = 0;
            }
        }

        bool RayTest(GameObject raySource, int i)
        {
            LightSource tempLight = raySource.GetComponent<LightSource>();
            LightbeamScript tempBeam = lines[j].GetComponent<LightbeamScript>();
            LineRenderer tempLine = lines[j].GetComponent<LineRenderer>();
            RaycastHit hit;
            if (Physics.Raycast(raySource.transform.position, raySource.transform.forward, out hit, Mathf.Infinity))
            {

                incidenceAngle = hit.point - raySource.transform.position;
                reflectionAngle = Vector3.Reflect(incidenceAngle, hit.normal);
                Debug.DrawRay(raySource.transform.position, hit.point - raySource.transform.position, Color.white);
                tempLine.enabled = true;
                tempLine.SetPosition(0, raySource.transform.position);
                tempLine.SetPosition(1, hit.point);
                tempBeam.changeColor(tempLight.colour);
                //linesScripts[i].enabled = true;
                //mesh[i].enabled = true;
                //linesScripts[i].m_startPos = raySource.transform.position;
                //linesScripts[i].m_endPos = hit.point;
                //linesScripts[i].SetStartAndEndPoints(raySource.transform.position, hit.point);

                if (hit.transform.tag == "Mirror")
                {
                    j++;
                    MirrorRayTest(hit, j, tempLight.colour);
                }
                else
                {
                    j++;
                }
                return true;

            }
            
            return false;
        }

        bool MirrorRayTest(RaycastHit raySource, int i, Color colour)
        {
            LightbeamScript tempBeam = lines[j].GetComponent<LightbeamScript>();
            LineRenderer tempLine = lines[j].GetComponent<LineRenderer>();
            RaycastHit hit;
            if (Physics.Raycast(raySource.point, reflectionAngle, out hit, Mathf.Infinity))
            {
                incidenceAngle = hit.point - raySource.point;
                Debug.DrawRay(raySource.point, incidenceAngle, Color.white);
                tempLine.enabled = true;
                tempLine.SetPosition(0, raySource.point);
                tempLine.SetPosition(1, hit.point);
                tempBeam.changeColor(colour);
                //linesScripts[i].enabled = true;
                //mesh[i].enabled = true;
                //linesScripts[i].m_startPos = raySource.transform.position;
                //linesScripts[i].m_endPos = hit.point;
                //linesScripts[i].SetStartAndEndPoints(raySource.transform.position, hit.point);
                reflectionAngle = Vector3.Reflect(incidenceAngle, hit.normal);
                if (hit.transform.tag == "Mirror")
                {
                    j++;
                    MirrorRayTest(hit, j, colour);
                }
                else if (hit.transform.tag == "PuzzleKey")
                {
                    KeyRayTest(hit, j);
                }
                else
                {
                    j++;
                }
                return true;
            }
            
            return false;
        }

        bool PrismRayTest(RaycastHit raySource, int i, Color colour)
        {
            LightbeamScript tempBeam = lines[j].GetComponent<LightbeamScript>();
            LineRenderer tempLine = lines[j].GetComponent<LineRenderer>();
            PrismScript tempPrism = raySource.collider.GetComponent<PrismScript>();
            RaycastHit hit;
            if (Physics.Raycast(raySource.point, reflectionAngle, out hit, Mathf.Infinity))
            {
                incidenceAngle = hit.point - raySource.point;
                Debug.DrawRay(raySource.point, incidenceAngle, Color.white);
                tempLine.enabled = true;
                tempLine.SetPosition(0, raySource.point);
                tempLine.SetPosition(1, hit.point);
                tempBeam.changeColor(colour);
                //linesScripts[i].enabled = true;
                //mesh[i].enabled = true;
                //linesScripts[i].m_startPos = raySource.transform.position;
                //linesScripts[i].m_endPos = hit.point;
                //linesScripts[i].SetStartAndEndPoints(raySource.transform.position, hit.point);
                reflectionAngle = Vector3.Reflect(incidenceAngle, hit.normal);
                if (hit.transform.tag == "Mirror")
                {
                    j++;
                    MirrorRayTest(hit, j, colour);
                }
                else if (hit.transform.tag == "PuzzleKey")
                {
                    KeyRayTest(hit, j);
                }
                else
                {
                    j++;
                }
                return true;
            }

            return false;
        }

        void KeyRayTest(RaycastHit raySource, int i)
        {
            KeyScript key = raySource.collider.GetComponent<KeyScript>();
            key.lightOnKey = true;
        }   

    }


}






