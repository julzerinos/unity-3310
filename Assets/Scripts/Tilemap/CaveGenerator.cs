using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Tilemap
{
    public class CaveGenerator : MonoBehaviour
    {
        public int width;
        public int height;
        public TileBase[] tiles;
        [Range(0, 1)] public float randomFillPercent;

        public GameObject portal;
        public GameObject player;
        public GameObject[] spawnables;
        public float[] spawnChances;

        private UnityEngine.Tilemaps.Tilemap _tm;

        private int[,] _map;

        private void Awake()
        {
            _tm = GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
            GenerateMap();
            SetSpawns();
            FillTilemap();
        }

        private void Start()
        {
            AudioController.Instance.StopSound();
        }

        private void SetSpawns()
        {
            var playerSpawn = new Vector2Int(Random.Range(3, width / 5), 5);
            var portalSpawn = new Vector2Int(Random.Range(4 * width / 5, width - 4), height - 5);

            SpawnObject(playerSpawn, player, true);
            SpawnObject(portalSpawn, portal, true);

            EnsurePath(playerSpawn, portalSpawn, 1, Random.Range(1, 6));
        }

        private void EnsurePath(Vector2Int s, Vector2Int d, int thickness, int segments)
        {
            var points = new List<Vector2Int> {s};

            for (var i = 1; i <= segments; i++)
                points.Add(new Vector2Int(
                    Random.Range(points[i - 1].x, points[i - 1].x + width / (segments + 2)),
                    Random.Range(0 + thickness + 1, height - thickness - 1)
                ));

            points.Add(d);

            for (var j = 0; j < points.Count - 1; j++)
                ClearPath(points[j], points[j + 1], thickness);
            ClearPath(points[points.Count - 2], d, thickness);
        }

        private void ClearPath(Vector2Int s, Vector2Int d, int thickness)
        {
            var dx = Mathf.Abs(d.x - s.x);
            var sx = d.x > s.x ? 1 : -1;
            var dy = Mathf.Abs(d.y - s.y);
            var sy = d.y > s.y ? 1 : -1;

            var e = (dx > dy ? dx : -dy) / 2;

            var x = s.x;
            var y = s.y;

            do
            {
                for (var i = x - thickness; i <= x + thickness; i++)
                for (var j = y - thickness; j <= y + thickness; j++)
                    _map[i, j] = 0;

                var e2 = e;

                if (e2 > -dx)
                {
                    e -= dy;
                    x += sx;
                }

                if (e2 < dy)
                {
                    e += dx;
                    y += sy;
                }
            } while (x != d.x || y != d.y);
        }

        private void SpawnObject(Vector2Int v, GameObject obj, bool makeSpace)
        {
            var spawnX = v.x;
            var spawnY = v.y;

            if (makeSpace)
                for (var i = spawnX - 1; i <= spawnX + 1; i++)
                for (var j = spawnY - 1; j <= spawnY + 1; j++)
                    _map[i, j] = 0;

            var g = Instantiate(
                obj,
                _tm.CellToWorld(new Vector3Int(spawnX, spawnY, 0)) + new Vector3(.1f, .15f, 0),
                Quaternion.identity
            );
            g.transform.parent = transform;
        }

        private void FillTilemap()
        {
            TransformArrays(out var pos, out var tileMap);
            _tm.SetTiles(pos, tileMap);
        }

        private void TransformArrays(out Vector3Int[] pos, out TileBase[] tileMap)
        {
            pos = new Vector3Int[width * height];
            tileMap = new TileBase[width * height];

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                pos[x + width * y] = new Vector3Int(x, y, 0);

                if (_map[x, y] <= 0)
                {
                    tileMap[x + width * y] = null;

                    if (Random.value >= 0.15f)
                        continue;

                    var type = Random.value;
                    float chance = 0;

                    for (var i = 0; i < spawnables.Length; i++)
                    {
                        if (type > chance && type < spawnChances[i])
                            SpawnObject(new Vector2Int(x, y), spawnables[Random.Range(0, spawnables.Length)], false);
                        else
                            chance += spawnChances[i];
                    }
                }
                else
                    tileMap[x + width * y] = GetTile();
            }
        }

        private TileBase GetTile() => tiles[Random.Range(0, tiles.Length)];

        private void GenerateMap()
        {
            _map = new int[width + 2, height + 2];
            RandomFillMap();

            for (var i = 0; i < 5; i++)
                SmoothMap();
        }

        private void RandomFillMap()
        {
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                if (x <= 1 || x >= width - 2 || y <= 1 || y >= width - 2)
                    _map[x, y] = 1;

                else
                    _map[x, y] = Random.value < randomFillPercent ? 1 : 0;
            }
        }

        private void SmoothMap()
        {
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    _map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    _map[x, y] = 0;
            }
        }

        private int GetSurroundingWallCount(int gridX, int gridY)
        {
            var wallCount = 0;
            for (var neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            for (var neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX < 0 || neighbourX >= width || neighbourY < 0 || neighbourY >= height)
                {
                    wallCount++;
                    continue;
                }

                wallCount += _map[neighbourX, neighbourY];
            }

            return wallCount;
        }
    }
}