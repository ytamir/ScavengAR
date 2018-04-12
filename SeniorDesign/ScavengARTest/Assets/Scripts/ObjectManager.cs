
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{

    private static ObjectManager om;

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
    public GameObject gd;
    public Dictionary<string, Vector2d> availableObjects = new Dictionary<string, Vector2d>();

    private bool first = true;

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

    private void Awake()
    {
        if (om == null)
        {
            om = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyObject(om);
        }
    }
    void Start()
    {
        gd = GameObject.Find("GameDriver");
        LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        _map.OnInitialized += () => _isInitialized = true;
        if(first)
        {
            StartCoroutine("SetLocation");
            first = false;
        }
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ScoreObject(string name)
    {

    }

    IEnumerator SetLocation()
    {
        while (GameObject.Find("GameDriver").GetComponent<GameDriver>().hostLocation.Equals(origin))
        {
            yield return new WaitForSeconds(1);
        }
        baseLocation = GameObject.Find("GameDriver").GetComponent<GameDriver>().hostLocation;
        for (int i = 0; i < gd.GetComponent<GameDriver>().objectCount; i++)
        {
            var oldBase = baseLocation;
            baseLocation.x = baseLocation.x + gd.GetComponent<GameDriver>().objLocations[0];
            baseLocation.y = baseLocation.y + gd.GetComponent<GameDriver>().objLocations[1];
            gd.GetComponent<GameDriver>().objLocations.RemoveAt(0);
            gd.GetComponent<GameDriver>().objLocations.RemoveAt(0);
            availableObjects.Add("Object " + i, baseLocation);
            GameObject test = Instantiate(obj, Conversions.GeoToWorldPosition(baseLocation.x, baseLocation.y,
                                                                    _map.CenterMercator,
                                                                    _map.WorldRelativeScale).ToVector3xz(), Quaternion.identity).gameObject;
            test.name = "Object " + i;
            test.transform.position = new Vector3(test.transform.position.x, 3, test.transform.position.z);
            test.transform.parent = GameObject.Find("ObjectStorage").transform;
            baseLocation = oldBase;
        }
    }

    public Vector2d getObjectCoords(string name)
    {
        Vector2d value;
        availableObjects.TryGetValue(name, out value);
        return value;
    }

    public void LoadCameraView()
    {

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
    }

    void Update()
    {

    }
}