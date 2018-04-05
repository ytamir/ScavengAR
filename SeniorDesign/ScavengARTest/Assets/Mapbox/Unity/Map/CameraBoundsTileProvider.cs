namespace Mapbox.Unity.Map
{
	using System.Linq;
	using UnityEngine;
	using Mapbox.Map;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;

	public class CameraBoundsTileProvider : AbstractTileProvider
	{
		[SerializeField]
		Camera _camera;

		// TODO: change to Vector4 to optimize for different aspect ratios.
		[SerializeField]
		int _visibleBuffer;

		[SerializeField]
		int _disposeBuffer;

		[SerializeField]
		float _updateInterval;

		Plane _groundPlane;
		Ray _ray;
		float _hitDistance;
		Vector3 _viewportTarget;
		float _elapsedTime;
		bool _shouldUpdate;

		Vector2d _currentLatitudeLongitude;
		UnwrappedTileId _cachedTile;
		UnwrappedTileId _currentTile;

		internal override void OnInitialized()
		{
			_groundPlane = new Plane(Vector3.up, Mapbox.Unity.Constants.Math.Vector3Zero);
			_viewportTarget = new Vector3(0.5f, 0.5f, 0);
			_shouldUpdate = true;
		}

        public void DisableRenderOnChildren(GameObject go)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                for (int j = 0; j < go.transform.GetChild(i).childCount; j++)
                {
                    go.transform.GetChild(i).GetChild(j).GetComponent<Renderer>().enabled = false;
                }
            }
            if(go.transform.GetComponent<Renderer>() != null)
            {
                go.transform.GetComponent<Renderer>().enabled = false;
            }
        }

        public void EnableRendererOnChildren(GameObject go)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                for (int j = 0; j < go.transform.GetChild(i).childCount; j++)
                {
                    if(go.transform.GetChild(i).GetChild(j).name!="CollisionRadius")
                    {
                        go.transform.GetChild(i).GetChild(j).GetComponent<Renderer>().enabled = true;
                    }
                }
            }
            if (go.transform.GetComponent<Renderer>() != null && go.name != "LocationManager")
            {
                go.transform.GetComponent<Renderer>().enabled = true;
            }
        }

		void Update()
		{
			if (!_shouldUpdate)
			{
				return;
			}

			_elapsedTime += Time.deltaTime;
			if (_elapsedTime >= _updateInterval)
			{
				_elapsedTime = 0f;
				_ray = _camera.ViewportPointToRay(_viewportTarget);
				if (_groundPlane.Raycast(_ray, out _hitDistance))
				{
					_currentLatitudeLongitude = _ray.GetPoint(_hitDistance).GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
					_currentTile = TileCover.CoordinateToTileId(_currentLatitudeLongitude, _map.Zoom);

					if (!_currentTile.Equals(_cachedTile))
					{
						// FIXME: this results in bugs at world boundaries! Does not cleanly wrap. Negative tileIds are bad.
						for (int x = _currentTile.X - _visibleBuffer; x <= (_currentTile.X + _visibleBuffer); x++)
						{
							for (int y = _currentTile.Y - _visibleBuffer; y <= (_currentTile.Y + _visibleBuffer); y++)
							{
								AddTile(new UnwrappedTileId(_map.Zoom, x, y));
							}
						}
						_cachedTile = _currentTile;
						Cleanup(_currentTile);
					}
				}
			}
		}

		void Cleanup(UnwrappedTileId currentTile)
		{
			var keys = _activeTiles.Keys.ToList();
			for (int i = 0; i < keys.Count; i++)
			{
				var tile = keys[i];
				bool dispose = false;
				dispose = tile.X > currentTile.X + _disposeBuffer || tile.X < _currentTile.X - _disposeBuffer;
				dispose = dispose || tile.Y > _currentTile.Y + _disposeBuffer || tile.Y < _currentTile.Y - _disposeBuffer;

				if (dispose)
				{
					RemoveTile(tile);
				}
			}
		}
	}
}
