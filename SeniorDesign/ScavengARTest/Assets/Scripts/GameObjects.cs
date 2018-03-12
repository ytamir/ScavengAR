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

        void Start()
        {
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            _map.OnInitialized += () => _isInitialized = true;
            StartCoroutine("SetLocation");
        }

        IEnumerator SetLocation()
        {
            yield return new WaitForSeconds(1);
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
                _isInitialized = false;
            }
        }

        void Update()
        {
            //transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Mapbox.Unity.Map;
//using Mapbox.Unity.Location;
//using Mapbox.Utils;
//using Mapbox.Unity.Utilities;

//public class GameObject : MonoBehaviour
//{

//    [SerializeField]
//    private AbstractMap _map;

//    bool _isInitialized;
//    Vector2d newLoc;
//    //GameObject cube;
//    public Transform cube;
//    public GameObject newestCube;

//    ILocationProvider _locationProvider;
//    ILocationProvider LocationProvider
//    {
//        get
//        {
//            if (_locationProvider == null)
//            {
//                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
//            }

//            return _locationProvider;
//        }
//    }

//    Vector3 initialLoc;
//    List<GameObject> obj = new List<GameObject>();
//    List<Vector2d> objLocations = new List<Vector2d>();
//    int seed;

//    private void Awake()
//    {
//        DontDestroyOnLoad(transform.gameObject);
//    }

//    // Use this for initialization
//    void Start()
//    {
//        _map.OnInitialized += () => _isInitialized = true;
//        LocationProvider.
//        StartCoroutine(SetLocation());
//        initialLoc = Conversions.GeoToWorldPosition(LocationProvider.Curre.LatitudeLongitude, _map.CenterMercator, _map.WorldRelativeScale);

//    }

//    IEnumerator SetLocation()
//    {
//        yield return new WaitForSeconds(1);
//        for (int i = 0; i < 10; i++)
//        {
//            Vector2 testLoc = Random.insideUnitCircle;
//            testLoc.x *= (float)0.0024;
//            testLoc.y *= (float)0.0018;
//            //testLoc.x *= (float)0.00024;
//            //testLoc.y *= (float)0.00018;
//            newLoc = LocationProvider.CurrentLocation.LatitudeLongitude;
//            newLoc.x = newLoc.x + testLoc.x;
//            newLoc.y = newLoc.y + testLoc.y;
//            obj.Add(Instantiate(cube, _map.GeoToWorldPosition(newLoc), Quaternion.identity).gameObject);
//            obj[i].transform.position = _map.GeoToWorldPosition(newLoc);
//            objLocations.Add(newLoc);
//        }
//        Debug.Log(newestCube.transform.position);
//        /*cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        cube.transform.localScale = new Vector3(3, 20, 3);
//        cube.transform.position = _map.GeoToWorldPosition(newLoc);
//        cube.name = "Cube";
//        Debug.Log(cube.transform.position.y);*/
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {

//        if (_isInitialized)
//        {
//            for (int i = 0; i < 10; i++)
//            {
//                obj[i].transform.position = _map.GeoToWorldPosition(objLocations[i]);
//            }
//            //newestCube.transform.position = _map.GeoToWorldPosition(newLoc);
//        }
//    }
//}
