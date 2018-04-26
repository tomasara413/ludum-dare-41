using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {

        public float cameraSpeed = 100f; //rychlost pohybu kamery
        public float screenBorder = 10f; //když najedeš myškou na hranice obrazovky, kamera se bude pohybovat
        public float scrollSpeed = 10f; // Rychlost Zoomu
        public float smoothness = 0.25f;
        public float rotationSpeed = 100f;

        private Quaternion targetRotation;
        private Vector3 targetPosition;

        public GameObject bounds;
        float xMin, xMax, yMin, yMax, zMin, zMax;

        private BuildingManager bm;
        private ObjectManager om;
        public ObjectList teamObjectList;

        public GameObject FogOfWar;
        private Renderer rend;
        private Shader FOW;
        private Camera cam;
        private int layerMask;

        protected Animator animator;
        private void Start()
        {
            GameObject managers = GameObject.FindGameObjectWithTag("Managers");
            bm = managers.GetComponent<BuildingManager>();
            om = managers.GetComponent<ObjectManager>();
            teamObjectList = om.GetTeamObjectList();
            rend = FogOfWar.GetComponent<Renderer>();
            cam = GetComponent<Camera>();
            FOW = Shader.Find("Unlit/FogOfWar");
            layerMask = LayerMask.GetMask("FogOfWar");
            animator = GetComponent<Animator>();
        }

        private Collider[] seenUnseenFrustrum;
        private List<Entity> selectedElements = new List<Entity>();
        private bool blockSelection = false;

        void Update()
        {
            previousPosition = transform.position;
            previousRotation = transform.rotation;
            UpdateMovement();
            UpdateFOW();
            CameraSelection();
        }

        void UpdateMovement()
        {
            float translation = cameraSpeed * Time.deltaTime;

            targetPosition = transform.position;
            if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - screenBorder)
                targetPosition += translation * new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= screenBorder)
                targetPosition -= translation * new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= screenBorder)
                targetPosition -= translation * new Vector3(transform.right.x, 0, transform.right.z).normalized;
            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - screenBorder)
                targetPosition += translation * new Vector3(transform.right.x, 0, transform.right.z).normalized;

            if (Input.GetKey(KeyCode.Q))
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 10 * rotationSpeed * Time.deltaTime, transform.rotation.eulerAngles.z);
            if (Input.GetKey(KeyCode.E))
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 10 * rotationSpeed * Time.deltaTime, transform.rotation.eulerAngles.z);

            float scroll = Input.mouseScrollDelta.y;   //zjistí input scrollu na myšce
            targetPosition += transform.forward * scroll * scrollSpeed * Time.deltaTime;    //zoomuje

            if (bounds)
            {
                //kontroluje přítomnost v krychli objektu bounds
                xMin = bounds.transform.position.x - bounds.transform.localScale.x / 2;
                xMax = bounds.transform.position.x + bounds.transform.localScale.x / 2;
                yMin = bounds.transform.position.y - bounds.transform.localScale.y / 2;
                yMax = bounds.transform.position.y + bounds.transform.localScale.y / 2;
                zMin = bounds.transform.position.z - bounds.transform.localScale.z / 2;
                zMax = bounds.transform.position.z + bounds.transform.localScale.z / 2;

                bool isMin;
                if ((isMin = xMin > targetPosition.x) || xMax < targetPosition.x)
                    targetPosition.x = isMin ? xMin : xMax;
                if ((isMin = yMin > targetPosition.y) || yMax < targetPosition.y)
                    targetPosition.y = isMin ? yMin : yMax;
                if ((isMin = zMin > targetPosition.z) || zMax < targetPosition.z)
                    targetPosition.z = isMin ? zMin : zMax;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1 - smoothness);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 - smoothness);
        }

        void CameraSelection()
        {
            if (transform.position != previousPosition || transform.rotation != previousRotation)
                RecalculateCameraFrustrum();

            seenUnseenFrustrum = Physics.OverlapBox(camVector, size, cam.transform.rotation);

            if (Input.GetMouseButton(0))
            {
                if (!blockSelection)
                    SelectUnits();
            }
            else
            {
                m1 = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
                RelocateSelectedUnits();
        }

        private void SelectUnits()
        {
            selectedElements.Clear();

            for (int i = 0; i < seenUnseenFrustrum.Length; i++)
            {
                if (IsWithinSelectionBounds(m1, Input.mousePosition, seenUnseenFrustrum[i]))
                {
                    //Debug.Log("unit" + i + ": " + seenUnseenFrustrum[i].name);
                    if ((e = seenUnseenFrustrum[i].GetComponent<Entity>()) && e.team == 0)
                        selectedElements.Add((Entity)e);
                }
            }
        }

        private bool isTeamObject;
        private void RelocateSelectedUnits()
        {
            if (selectedElements.Count > 0)
            {
                RaycastHit rh;

                Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out rh, cam.farClipPlane, bm.TerrainMask);

                if (rh.collider != null)
                {
                    if (e2 = rh.collider.gameObject.GetComponent<TeamObject>())
                        isTeamObject = true;

                    for (int i = 0; i < selectedElements.Count; i++)
                    {
                        e = selectedElements[i];
                        if (e is Entity)
                        {
                            if (isTeamObject)
                            {
                                if (e.team != e2.team)
                                    (e as Entity).Attack(e2.gameObject);
                                else
                                {
                                    if (Input.GetKey(KeyCode.LeftShift))
                                        (e as Entity).Follow(e2.gameObject);
                                    else
                                        (e as Entity).SetDestination(rh.point);
                                }

                            }
                            else
                            {
                                if (Input.GetKey(KeyCode.LeftShift))
                                    (e as Entity).AddPathPoint(rh.point);
                                else
                                    (e as Entity).SetDestination(rh.point);
                            }
                        }
                    }

                    isTeamObject = false;
                }
            }
        }

        Vector4[] camVectors;
        float[] distances;
        int previousCount = 0;
        RaycastHit rh;

        List<GameObject> teamObs;
        void UpdateFOW()
        {
            if (teamObs == null)
                teamObs = teamObjectList.GetListOfVisionObjects(0);

            if (teamObs != null)
            {
                //Debug.Log(teamObs.Count);
                camVectors = new Vector4[teamObs.Count == 0 ? 1 : teamObs.Count];
                distances = new float[teamObs.Count == 0 ? 1 : teamObs.Count];
                for (int i = 0; i < teamObs.Count; i++)
                {
                    Debug.DrawRay(transform.position, teamObs[i].transform.position - transform.position);
                    if (Physics.Raycast(transform.position, teamObs[i].transform.position - transform.position, out rh, cam.farClipPlane, layerMask))
                        camVectors[i] = rh.point;
                    //Debug.Log(teamObs[i].GetComponent<TeamObject>().VisionRange);
                    distances[i] = teamObs[i].GetComponent<TeamObject>().VisionRange;
                }

                rend.material.SetInt("_VectorsCount", teamObs.Count);
                if (previousCount != teamObs.Count)
                {
                    rend.material = new Material(FOW);

                    previousCount = teamObs.Count;
                }

                rend.material.SetVectorArray("_Vectors", camVectors);
                rend.material.SetFloatArray("_Distances", distances);
            }
        }

        #region CameraFunctions

        protected Collider c;
        protected TeamObject e, e2;

        private Color darkBorder = new Color(1, 1, 1, 30f / 255f);
        private Color fill = new Color(1, 1, 1, 30f / 255f);

        private void OnGUI()
        {
            //Debug.Log("mouse: " + m1.x);
            if (Input.GetMouseButton(0))
                Utils.DrawScreenRectFilled(Utils.GetScreenRect(m1, Input.mousePosition), 1, darkBorder, fill);

            if (seenUnseenFrustrum != null)
            {
                //Debug.Log(seenUnseenFrustrum.Length);
                for (int i = 0; i < seenUnseenFrustrum.Length; i++)
                {
                    if (IsWithinSelectionBounds(Vector2.zero, cam.ViewportToScreenPoint(new Vector3(1, 1)), seenUnseenFrustrum[i]))
                    {
                        if (e = seenUnseenFrustrum[i].GetComponent<Entity>())
                        {
                            c = e.GetComponent<Collider>();

                            workingVector = cam.WorldToScreenPoint(c.bounds.center);
                            GUI.Label(new Rect(new Vector2(workingVector.x - 15, Screen.height - workingVector.y - 40), GUI.skin.label.CalcSize(new GUIContent(e.Health.ToString()))), new GUIContent(e.Health.ToString()));
                        }
                    }
                }
            }
        }

        private Vector3 m1, camVector, workingVector, workingVector2, size;
        private Vector3[] corners = new Vector3[4];
        //private Vector3[] pointsToUse = new Vector3[2];
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        private bool IsWithinSelectionBounds(Vector3 mouse1, Vector3 mouse2, Collider c)
        {
            if (c == null)
                return false;

            workingVector = mouse1;
            mouse1 = cam.ScreenToViewportPoint(Vector3.Min(mouse1, mouse2));
            mouse2 = cam.ScreenToViewportPoint(Vector3.Max(workingVector, mouse2));
            mouse1.z = cam.nearClipPlane;
            mouse2.z = cam.farClipPlane;

            Bounds viewportBounds = new Bounds();
            viewportBounds.SetMinMax(mouse1, mouse2);

            if (viewportBounds.Contains(cam.WorldToViewportPoint(c.gameObject.transform.position)))
                return true;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        workingVector = c.bounds.center;
                        workingVector.x = workingVector.x + (c.bounds.max.x - c.bounds.center.x) * i;
                        workingVector.y = workingVector.y + (c.bounds.max.y - c.bounds.center.y) * j;
                        workingVector.z = workingVector.z + (c.bounds.max.z - c.bounds.center.z) * k;

                        if (viewportBounds.Contains(cam.WorldToViewportPoint(workingVector)))
                            return true;
                    }
                }
            }
            return false;
        }

        private void RecalculateCameraFrustrum()
        {
            cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, corners);
            camVector = new Vector3();

            //GameObject g;
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = gameObject.transform.position + gameObject.transform.TransformVector(corners[i]);
                camVector += corners[i];
            }

            for (int i = 0; i < corners.Length; i++)
            {
                camVector += corners[i] - cam.transform.forward * (cam.farClipPlane - cam.nearClipPlane);
            }

            camVector /= corners.Length * 2;

            size.x = (corners[0] - corners[3]).magnitude / 2;
            size.y = (corners[0] - corners[1]).magnitude / 2;
            size.z = (cam.transform.forward * (cam.farClipPlane - cam.nearClipPlane)).magnitude / 2;
        }

        #endregion
    }
}