using Microsoft.Extensions.Logging;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Game;

[DebuggerDisplay("{Name} (Id={Id})")]
public sealed class Map : IDisposable
{
    private static readonly int FrameRate = 67;
    private static readonly float UpdateRate = 1000f / FrameRate;

    private readonly ILogger<Map> _logger;
    private readonly CancellationTokenSource _mainProcessTaskCancelTokenSource = new();
    private readonly List<MapLayer> _layers = new();
    private readonly MapLayer _defaultMapLayer;
    private int _mapLayerIdGenerator = 1;

    /// <summary>
    /// Gets the map properties.
    /// </summary>
    public MapProperties Properties { get; }

    /// <summary>
    /// Gets the map id.
    /// </summary>
    public int Id => Properties.Id;

    /// <summary>
    /// Gets the map name.
    /// </summary>
    public string Name => Properties.Name;

    private CancellationToken MainProcessTaskCancelToken => _mainProcessTaskCancelTokenSource.Token;

    public Map(MapProperties properties, ILogger<Map> logger)
    {
        Properties = properties;
        _logger = logger;
        _defaultMapLayer = new MapLayer(this, layerId: _mapLayerIdGenerator++);

        _layers.Add(_defaultMapLayer);

        Task.Factory.StartNew(UpdateAsync, MainProcessTaskCancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        Task.Factory.StartNew(UpdateSecondsAsync, MainProcessTaskCancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    /// <summary>
    /// Gets the default map layer.
    /// </summary>
    /// <returns>The default map layer.</returns>
    public MapLayer GetDefaultLayer() => _defaultMapLayer;

    /// <summary>
    /// Gets a map layer by its id.
    /// </summary>
    /// <param name="layerId">Layer id.</param>
    /// <returns>The layer if found; the default map layer otherwise.</returns>
    public MapLayer GetLayer(int layerId) => _layers.SingleOrDefault(x => x.Id == layerId);

    /// <summary>
    /// Gets the map height at a given X and Z position.
    /// </summary>
    /// <param name="positionX">X coordinate.</param>
    /// <param name="positionZ">Z coordinate.</param>
    /// <returns>Height.</returns>
    public float GetHeight(float positionX, float positionZ)
    {
        // TODO
        return 0;
    }

    /// <summary>
    /// Check if the given X,Y,Z coordinates are in map bounds.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <returns>True if the position is in map bounds; false otherwise.</returns>
    public bool IsInBounds(float x, float y, float z) => Properties.Bounds.Contains(x, y, z);

    /// <summary>
    /// Check if the given position is in map bounds.
    /// </summary>
    /// <param name="position">Position.</param>
    /// <returns>True if the position is in map bounds; false otherwise.</returns>
    public bool IsInBounds(Vector3 position) => IsInBounds(position.X, position.Y, position.Z);

    private async Task UpdateAsync()
    {
        while (!MainProcessTaskCancelToken.IsCancellationRequested)
        {
            try
            {
                var nextUpdate = DateTime.UtcNow.AddMilliseconds(UpdateRate);

                lock (_layers)
                {
                    foreach (MapLayer layer in _layers)
                    {
                        layer.Update();
                    }
                }

                var currentTime = DateTime.UtcNow;

                if (nextUpdate > currentTime)
                {
                    await Task.Delay((nextUpdate - currentTime).Milliseconds, MainProcessTaskCancelToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured on map '{Name}'.");
            }
        }
    }

    private async Task UpdateSecondsAsync()
    {
        while (!MainProcessTaskCancelToken.IsCancellationRequested)
        {
            lock (_layers)
            {
                foreach (MapLayer layer in _layers)
                {
                    layer.UpdateSeconds();
                }
            }

            await Task.Delay(1000, MainProcessTaskCancelToken);
        }
    }

    public void Dispose()
    {
        _mainProcessTaskCancelTokenSource.Cancel();

        foreach (MapLayer layer in _layers)
        {
            layer.Dispose();
        }

        _layers.Clear();
    }
}
