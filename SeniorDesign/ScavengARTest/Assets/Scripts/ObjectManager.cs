
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;
using UnityEngine;
using System.Collections;
using Mapbox.Utils;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private AbstractMap _map;

    /// <summary>
    /// The rate at which the transform's position tries catch up to the provided location.
    /// </summary>
    [SerializeField]
    float _positionFollowFactor;

    public GameObject obj;
    public Vector2d baseLocation = new Vector2d(0, 0);
    public Vector2d origin = new Vector2d(0, 0);

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
        DontDestroyOnLoad(this.gameObject);
        LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        _map.OnInitialized += () => _isInitialized = true;
        StartCoroutine("SetLocation");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
    }

    IEnumerator SetLocation()
    {
        while (baseLocation.Equals(origin))
        {
            Debug.Log("here");
            yield return new WaitForSeconds(1);
        }
        Debug.Log(baseLocation);
        for (int i = 0; i < 10; i++)
        {
            var oldBase = baseLocation;
            Vector2 testLoc = Random.insideUnitCircle;
            var t1 = GameObject.FindGameObjectWithTag("GameDriver").GetComponent<GameDriver>().getX();
            Debug.Log(t1);
            var t2 = GameObject.FindGameObjectWithTag("GameDriver").GetComponent<GameDriver>().getY();
            Debug.Log(t2);
            testLoc.x *= t1;
            testLoc.y *= t2;
            baseLocation.x = baseLocation.x + testLoc.x;
            baseLocation.y = baseLocation.y + testLoc.y;
            GameObject test = Instantiate(obj, Conversions.GeoToWorldPosition(baseLocation.x, baseLocation.y,
                                                                    _map.CenterMercator,
                                                                    _map.WorldRelativeScale).ToVector3xz(), Quaternion.identity).gameObject;
            test.transform.position = new Vector3(test.transform.position.x, 3, test.transform.position.z);
            
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
        /*if (_isInitialized)
        {
            baseLocation = e.Location;
            _targetPosition = Conversions.GeoToWorldPosition(e.Location,
                                                                _map.CenterMercator,
                                                                _map.WorldRelativeScale).ToVector3xz();
            Debug.Log("Obj: " + e.Location);
            _isInitialized = false;
        }*/
    }

    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
    }
}