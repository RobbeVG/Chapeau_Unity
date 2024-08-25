using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

namespace Seacore
{
    public class CreateFrustumCollidersWizard : ScriptableWizard
    {
        [Flags]
        enum Side
        {
            None,
            Left = 1,
            Right = 2,
            Down = 4,
            Up = 8,
            Near = 16,
            Far = 32,

        }

        [SerializeField]
        Camera _camera = null;
        [SerializeField]
        Side selectedSides = Side.None;

        [Header("Parameters")]
        [SerializeField][Range(0.1f, 100.0f)]
        float thickness = 1.0f;
        [Header("Unsaved Parameters")]
        [SerializeField]
        float offsetFromCamera = 0.0f;
        [SerializeField]
        float offsetFromNormal = 0.0f;
        
        #region Private variables
        const int c_PlaneCount = 6;
        const int c_CornerCount = 8;

        [Header("Debug information")]
        [SerializeField][ReadOnly]
        BoxCollider[] _colliders = new BoxCollider[c_PlaneCount];
        [SerializeField][ReadOnly]
        Vector3[] _frustumCorners = new Vector3[c_CornerCount];
        #endregion

        #region Wizard functions

        [MenuItem("Tools/Create Frustum Colliders")]
        static void CreateWizard()
        {
            DisplayWizard<CreateFrustumCollidersWizard>("Create Frustum Collider");
        }

        private void OnWizardUpdate()
        {
            CalculateCorners();

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

            for (int index = 0; index < _colliders.Length; index++)
            {
                Side side = (Side)Mathf.Pow(2, index);
                if (selectedSides.HasFlag(side))
                {
                    if (_colliders[index] == null)
                        _colliders[index] = CreateBoxCollider(side.ToString());
                    CalculateBoxCollider(_colliders[index], planes[index].normal, GetCorners(side));
                }
                else
                    DestroyBoxCollider(index);
            }
        }
        #endregion

        #region Event functions
        private void Awake()
        {
            //Camera
            if (_camera == null)
            {
                Assert.IsNotNull(Camera.main, "No main camera in te scene, please add one");
                _camera = Camera.main;
            }

            //Reverse engineer previous settings

            BoxCollider boxCollider = null;
            //Side + Colliders
            string[] names = Enum.GetNames(typeof(Side)).Skip(1).ToArray(); //skip none
            for (int childNr = 0; childNr < _camera.transform.childCount; childNr++)
            {
                Transform childTransform = _camera.transform.GetChild(childNr);
                int index = Array.IndexOf(names, childTransform.name);
                if (index != -1)
                {
                    selectedSides |= (Side)(1 << index); //To flag

                    boxCollider = _colliders[index] = childTransform.GetComponent<BoxCollider>();
                    if (_colliders[index] == null)
                        boxCollider = _colliders[index] = childTransform.gameObject.AddComponent<BoxCollider>();
                }
            }

            //Parameters
            if (boxCollider != null)
            {
                //CalculateCorners(); //Can we use to see previous corner
                //Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

                thickness = boxCollider.size.z;
            }
        }
        #endregion


        #region Calculations
        private void CalculateCorners()
        {
            //Far
            _frustumCorners[0] = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.farClipPlane)); //Side.Left     | Side.Down | Side.Far
            _frustumCorners[1] = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.farClipPlane)); //Side.Left     | Side.Up   | Side.Far
            _frustumCorners[2] = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.farClipPlane)); //Side.Right    | Side.Down | Side.Far
            _frustumCorners[3] = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.farClipPlane)); //Side.Right    | Side.Up   | Side.Far

            //Near
            _frustumCorners[4] = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane + offsetFromCamera)); //Side.Left     | Side.Down | Side.Near
            _frustumCorners[5] = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane + offsetFromCamera)); //Side.Left     | Side.Up   | Side.Near
            _frustumCorners[6] = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane + offsetFromCamera)); //Side.Right    | Side.Down | Side.Near
            _frustumCorners[7] = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane + offsetFromCamera)); //Side.Right    | Side.Up   | Side.Near
        }
        private void CalculateBoxCollider(BoxCollider collider, Vector3 normalPlane, Vector3[] corners) 
        {
            Assert.IsTrue(corners.Length == 4);

            Vector3 center = Vector3.zero;
            foreach (Vector3 point in corners)
                center += point;
            center /= 4;

            float disstanceY = 0;
            for (int i = 0; i < corners.Length; i+=2)
            {
                float distance = Vector3.Distance(corners[i], corners[i + 1]);
                if (disstanceY < distance)
                    disstanceY = distance;
            }
            float distanceX = 0;
            for (int i = 1; i < corners.Length - 1; i+=2)
            {
                float distance = Vector3.Distance(corners[i], corners[i + 1]);
                if (distanceX < distance)
                    distanceX = distance;
            }

            collider.transform.rotation = Quaternion.LookRotation(normalPlane);
            collider.transform.position = center + (offsetFromNormal * normalPlane);
            collider.transform.localScale = Vector3.one;
            collider.size = new Vector3(distanceX, disstanceY, thickness);
        }
        #endregion

        #region Helpers
        private Vector3[] GetCorners(Side side)
        {
            switch (side)
            {
                case Side.Left:
                    return new Vector3[] { _frustumCorners[0], _frustumCorners[1], _frustumCorners[4], _frustumCorners[5] };
                case Side.Right:
                    return new Vector3[] { _frustumCorners[2], _frustumCorners[3], _frustumCorners[6], _frustumCorners[7] };
                case Side.Down:
                    return new Vector3[] { _frustumCorners[0], _frustumCorners[2], _frustumCorners[4], _frustumCorners[6] };
                case Side.Up:
                    return new Vector3[] { _frustumCorners[1], _frustumCorners[3], _frustumCorners[5], _frustumCorners[7] };
                case Side.Near:
                    return new Vector3[] { _frustumCorners[4], _frustumCorners[5], _frustumCorners[6], _frustumCorners[7] };
                case Side.Far:
                    return new Vector3[] { _frustumCorners[0], _frustumCorners[1], _frustumCorners[2], _frustumCorners[3] };
            }

            return null;
        }
        private BoxCollider CreateBoxCollider(string childName)
        {
            GameObject child = new GameObject(childName);
            child.transform.SetParent(_camera.transform);
            return child.AddComponent<BoxCollider>();
        }
        private void DestroyBoxCollider(int index) 
        {
            if (_colliders[index] != null)
            {
                DestroyImmediate(_colliders[index].gameObject);
                _colliders[index] = null;
            }
        }
        #endregion
    }
}
