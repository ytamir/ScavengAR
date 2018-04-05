namespace Mapbox.Examples
{
    using Mapbox.Unity.Location;
    using Mapbox.Unity.Utilities;
    using Mapbox.Unity.Map;
    using UnityEngine;
    using System.Collections;
    using Mapbox.Utils;

    public class GameObjects : MonoBehaviour
    {
        [SerializeField]
        private AbstractMap _map;

        /// <summary>
        /// The rate at which the transform's position tries catch up to the provided location.
        /// </summary>
        [SerializeField]
        float _positionFollowFactor;

        public GameObject obj;
        public Vector2d baseLocation;

        /// <summary>
        /// Use a mock <see cref="T:Mapbox.Unity.Location.TransformLocationProvider"/>,
        /// rather than a <see cref="T:Mapbox.Unity.Location.EditorLocationProvider"/>. 
        /// </summary>
        [SerializeField]
        bool _useTransformLocationProvider;

        bool _isInitialized;

        /// <summary>
        /// The location provider.
        /// This is public so you change which concrete <see cref="T:Mapbox.Unity.Location.ILocationProvider"/> to use at runtime.
        /// </summary>
        ILocationProvider _locationProvider;
        public ILocationProvider LocationProvider
        {
            private get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = _useTransformLocationProvider ?
                        LocationProviderFactory.Instance.TransformLocationProvider : LocationProviderFactory.Instance.DefaultLocationProvider;
                }

                return _locationProvider;
            }
            set
            {
                if (_locationProvider != null)
                {
                    _locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;

                }
                _locationProvider = value;
                _locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            }
        }

        Vector3 _targetPosition;

        public void SetBaseLocation(Vector2d bl)
        {
            baseLocation = bl;
        }

        void Start()
        {
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            _map.OnInitialized += () => _isInitialized = true;
            StartCoroutine("SetLocation");
        }

        IEnumerator SetLocation()
        {
            yield return new WaitForSeconds(3);
            //Debug.Log(baseLocation);
            for (int i = 0; i < 10; i++)
            {
                var oldBase = baseLocation;
                Vector2 testLoc = Random.insideUnitCircle;
                testLoc.x *= (float)0.0024;
                testLoc.y *= (float)0.0018;
                baseLocation.x = 38.9572721;
                baseLocation.y = -95.2550833;
                baseLocation.x = baseLocation.x + testLoc.x;
                baseLocation.y = baseLocation.y + testLoc.y;
                GameObject test = Instantiate(obj, Conversions.GeoToWorldPosition(baseLocation.x, baseLocation.y,
                                                                     _map.CenterMercator,
                                                                     _map.WorldRelativeScale).ToVector3xz(), Quaternion.identity).gameObject;
                baseLocation = oldBase;
            }
                //test.transform.rotation = new Quaternion(0, 0, 0, 5);
            /*cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(3, 20, 3);
            cube.transform.position = _map.GeoToWorldPosition(newLoc);
            cube.name = "Cube";
            Debug.Log(cube.transform.position.y);*/
        }

        void OnDestroy()
        {
            if (LocationProvider != null)
            {
                LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            }
        }

        void LocationProvider_OnLocationUpdated(object sender, LocationUpdatedEventArgs e)
        {
            if (_isInitialized)
            {
                baseLocation = e.Location;
                _targetPosition = Conversions.GeoToWorldPosition(e.Location,
                                                                 _map.CenterMercator,
                                                                 _map.WorldRelativeScale).ToVector3xz();
                Debug.Log("Obj: " + e.Location);
                _isInitialized = false;
            }
        }

        void Update()
        {
            //transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
        }
    }
}