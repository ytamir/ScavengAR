
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;
using UnityEngine;
using Mapbox.Utils;

public class PositionWithLocationProvider : MonoBehaviour
{
	[SerializeField]
	private AbstractMap _map;

    public Vector2d curLoc;

	/// <summary>
	/// The rate at which the transform's position tries catch up to the provided location.
	/// </summary>
	[SerializeField]
	float _positionFollowFactor;

    bool temp = true;
    public GameObject ObjectManager;

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
        Debug.Log(_isInitialized);
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
            curLoc = e.Location;
			_targetPosition = Conversions.GeoToWorldPosition(e.Location,
																_map.CenterMercator,
																_map.WorldRelativeScale).ToVector3xz();
            
            if(temp)
            {
                GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>().SetBaseLocation(e.Location);
                transform.position = _targetPosition;
                temp = false;
            }
		}
	}

    public Vector2d getCurrentLoc()
    {
        return curLoc;
    }

	void Update()
	{
        //Debug.Log(GameObject.FindGameObjectWithTag("LocationManager").GetComponent<LocationManager>().getTargetPosition());
        if (_targetPosition != new Vector3(0, 0, 0))
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
        }
	}
}