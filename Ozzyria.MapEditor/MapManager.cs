using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.MapEditor.EventSystem;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Ozzyria.MapEditor
{
    class MapManager
    {
        protected static Map _map;

        public static bool MapIsLoaded()
        {
            return _map != null;
        }

        public static void LoadMap(Map map)
        {
            _map = map;

            EventQueue.Queue(new MapLoadedEvent
            {
                TileDimension = _map.TileDimension,
                Width = _map.Width,
                Height = _map.Height,
                NumberOfLayers = _map.layers.Count
            });
        }

        public static void BakeMap()
        {
            for (var layer = 0; layer < GetNumberOfLayers(); layer++)
            {
                for (var x = 0; x < _map.Width; x++)
                {
                    for (var y = 0; y < _map.Height; y++)
                    {
                        var tileType = GetTileType(layer, x, y);

                        // reset before recalculating
                        _map.SetTransitionType(layer, x, y, TransitionType.None);
                        _map.SetPathDirection(layer, x, y, PathDirection.None);

                        if (tileType == TileType.Water)
                        {
                            var leftIsGround = GetTileType(layer, x - 1, y) == TileType.Ground;
                            var rightIsGround = GetTileType(layer, x + 1, y) == TileType.Ground;
                            var upIsGround = GetTileType(layer, x, y - 1) == TileType.Ground;
                            var downIsGround = GetTileType(layer, x, y + 1) == TileType.Ground;

                            var upLeftIsGround = GetTileType(layer, x - 1, y - 1) == TileType.Ground;
                            var upRightIsGround = GetTileType(layer, x + 1, y - 1) == TileType.Ground;
                            var downLeftIsGround = GetTileType(layer, x - 1, y + 1) == TileType.Ground;
                            var downRightIsGround = GetTileType(layer, x + 1, y + 1) == TileType.Ground;

                            if (upIsGround && leftIsGround && upLeftIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpLeft);
                            }
                            else if (upIsGround && rightIsGround && upRightIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpRight);
                            }
                            else if (downIsGround && leftIsGround && downLeftIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownLeft);
                            }
                            else if (downIsGround && rightIsGround && downRightIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownRight);
                            }
                            else if (upIsGround && (upRightIsGround || upLeftIsGround))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Up);
                            }
                            else if (downIsGround && (downLeftIsGround || downRightIsGround))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Down);
                            }
                            else if (leftIsGround && (upLeftIsGround || downLeftIsGround))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Left);
                            }
                            else if (rightIsGround && (upRightIsGround || downRightIsGround))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Right);
                            }
                            else if (!downIsGround && !rightIsGround && downRightIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownRightDiagonal);
                            }
                            else if (!downIsGround && !leftIsGround && downLeftIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownLeftDiagonal);
                            }
                            else if (!upIsGround && !leftIsGround && upLeftIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpLeftDiagonal);
                            }
                            else if (!upIsGround && !leftIsGround && upRightIsGround)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpRightDiagonal);
                            }
                        }
                        else if (tileType == TileType.Fence || tileType == TileType.Road)
                        {
                            var leftIsPath = GetTileType(layer, x - 1, y) == tileType;
                            var rightIsPath = GetTileType(layer, x + 1, y) == tileType;
                            var upIsPath = GetTileType(layer, x, y - 1) == tileType;
                            var downIsPath = GetTileType(layer, x, y + 1) == tileType;

                            if (leftIsPath && !rightIsPath && !upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Left);
                            }
                            else if (!leftIsPath && rightIsPath && !upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Right);
                            }
                            else if (!leftIsPath && !rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Up);
                            }
                            else if (!leftIsPath && !rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Down);
                            }
                            else if (leftIsPath && rightIsPath && !upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.LeftRight);
                            }
                            else if (leftIsPath && rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpT);
                            }
                            else if (leftIsPath && rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.DownT);
                            }
                            else if (!leftIsPath && !rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpDown);
                            }
                            else if (leftIsPath && !rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.LeftT);
                            }
                            else if (!leftIsPath && rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.RightT);
                            }
                            else if (leftIsPath && !rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpLeft);
                            }
                            else if (!leftIsPath && rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpRight);
                            }
                            else if (!leftIsPath && rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.DownRight);
                            }
                            else if (leftIsPath && !rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.DownLeft);
                            }
                            else if (leftIsPath && rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.All);
                            }
                        }
                    }
                }
            }
        }

        public static void SaveMap()
        {
            if (!MapIsLoaded())
            {
                return;
            }

            var baseMapsDirectory = @"C:\Users\cgari\source\repos\Ozzyria\Ozzyria.Game\Maps"; // TODO this is just to make debuggery easier for now
            if (!System.IO.Directory.Exists(baseMapsDirectory))
            {
                System.IO.Directory.CreateDirectory(baseMapsDirectory);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(baseMapsDirectory + "\\test_m.ozz"))
            {
                file.WriteLine(_map.Width);
                file.WriteLine(_map.Height);
                file.WriteLine(GetNumberOfLayers());
                for (var layer = 0; layer < GetNumberOfLayers(); layer++)
                {
                    for (var x = 0; x < _map.Width; x++)
                    {
                        for (var y = 0; y < _map.Height; y++)
                        {
                            var tileType = GetTileType(layer, x, y);

                            var tx = 0;
                            var ty = 0;
                            if (tileType == TileType.None)
                            {
                                continue;
                            }
                            else if (tileType == TileType.Ground)
                            {
                                tx = 0;
                                ty = 0;
                            }
                            else if (tileType == TileType.Water)
                            {
                                // TODO need to.. centralize this tx, ty stuff?
                                switch (_map.GetTransitionType(layer, x, y))
                                {
                                    case TransitionType.UpLeft:
                                        tx = 1;
                                        ty = 0;
                                        break;
                                    case TransitionType.UpRight:
                                        tx = 3;
                                        ty = 0;
                                        break;
                                    case TransitionType.DownLeft:
                                        tx = 1;
                                        ty = 2;
                                        break;
                                    case TransitionType.DownRight:
                                        tx = 3;
                                        ty = 2;
                                        break;
                                    case TransitionType.Up:
                                        tx = 2;
                                        ty = 0;
                                        break;
                                    case TransitionType.Down:
                                        tx = 2;
                                        ty = 2;
                                        break;
                                    case TransitionType.Left:
                                        tx = 1;
                                        ty = 1;
                                        break;
                                    case TransitionType.Right:
                                        tx = 3;
                                        ty = 1;
                                        break;
                                    case TransitionType.DownRightDiagonal:
                                        tx = 2;
                                        ty = 3;
                                        break;
                                    case TransitionType.DownLeftDiagonal:
                                        tx = 3;
                                        ty = 3;
                                        break;
                                    case TransitionType.UpLeftDiagonal:
                                        tx = 3;
                                        ty = 4;
                                        break;
                                    case TransitionType.UpRightDiagonal:
                                        tx = 2;
                                        ty = 4;
                                        break;
                                    default:
                                        tx = 0;
                                        ty = 1;
                                        break;
                                }
                            }
                            else if (tileType == TileType.Fence || tileType == TileType.Road)
                            {
                                int baseTx = 0;
                                int baseTy = 0;
                                if (tileType == TileType.Road)
                                {
                                    // TODO right now this is a azy hack... probably store this data in a meta file for the tile set
                                    baseTx = 4;
                                    baseTy = 0;
                                }
                                // TODO need to.. centralize this tx, ty stuff?
                                switch (_map.GetPathDirection(layer, x, y))
                                {
                                    case PathDirection.Left:
                                        tx = 6;
                                        ty = 3;
                                        break;
                                    case PathDirection.Right:
                                        tx = 7;
                                        ty = 3;
                                        break;
                                    case PathDirection.Up:
                                        tx = 4;
                                        ty = 3;
                                        break;
                                    case PathDirection.Down:
                                        tx = 5;
                                        ty = 3;
                                        break;
                                    case PathDirection.LeftRight:
                                        tx = 5;
                                        ty = 0;
                                        break;
                                    case PathDirection.UpT:
                                        tx = 7;
                                        ty = 0;
                                        break;
                                    case PathDirection.DownT:
                                        tx = 6;
                                        ty = 0;
                                        break;
                                    case PathDirection.UpDown:
                                        tx = 6;
                                        ty = 2;
                                        break;
                                    case PathDirection.LeftT:
                                        tx = 7;
                                        ty = 2;
                                        break;
                                    case PathDirection.RightT:
                                        tx = 7;
                                        ty = 1;
                                        break;
                                    case PathDirection.UpLeft:
                                        tx = 5;
                                        ty = 2;
                                        break;
                                    case PathDirection.UpRight:
                                        tx = 4;
                                        ty = 2;
                                        break;
                                    case PathDirection.DownRight:
                                        tx = 4;
                                        ty = 1;
                                        break;
                                    case PathDirection.DownLeft:
                                        tx = 5;
                                        ty = 1;
                                        break;
                                    case PathDirection.All:
                                        tx = 4;
                                        ty = 0;
                                        break;
                                    default:
                                        tx = 6;
                                        ty = 1;
                                        break;
                                }
                                tx += baseTx;
                                ty += baseTy;
                            }
                            file.WriteLine($"{layer}|{x}|{y}|{tx}|{ty}");
                        }
                    }
                }
                file.WriteLine("END");
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(baseMapsDirectory + "\\test_e.ozz"))
            {
                var serializeOptions = new JsonSerializerOptions();

                var orb = new Entity();
                orb.AttachComponent(new Renderable { Sprite = SpriteType.Particle });
                orb.AttachComponent(new ExperienceOrbThought());
                orb.AttachComponent(new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = 400, Y = 300 });
                orb.AttachComponent(new ExperienceBoost { Experience = 30 });
                WriteEntity(file, serializeOptions, orb);

                var slime = new Entity();
                slime.AttachComponent(new Renderable { Sprite = SpriteType.Slime });
                slime.AttachComponent(new SlimeThought());
                slime.AttachComponent(new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = 500, Y = 400 });
                slime.AttachComponent(new Stats { Health = 30, MaxHealth = 30 });
                slime.AttachComponent(new Combat());
                slime.AttachComponent(new BoundingCircle { Radius = 10 });
                WriteEntity(file, serializeOptions, slime);

                var box = new Entity();
                box.AttachComponent(new Movement() { X = 60, Y = 60, PreviousX = 60, PreviousY = 60 });
                box.AttachComponent(new BoundingCircle() { IsDynamic = false, Radius = 10 });
                WriteEntity(file, serializeOptions, box);


                // wrap screen in border
               WriteWall(file, serializeOptions, 400, 20, 900, 10);
               WriteWall(file, serializeOptions, 400, 510, 900, 10);
               WriteWall(file, serializeOptions, 20, 300, 10, 700);
               WriteWall(file, serializeOptions, 780, 300, 10, 700);

               WriteWall(file, serializeOptions, 150, 300, 400, 10);
               WriteWall(file, serializeOptions, 200, 300, 10, 300);

                file.WriteLine("END");
            }
        }

        private static void WriteWall(System.IO.StreamWriter file, JsonSerializerOptions serializeOptions, float x, float y, int w, int h) // TODO move somewhere else
        {
            var wall = new Entity();
            wall.AttachComponent(new Movement() { X = x, Y = y, PreviousX = x, PreviousY = y });
            wall.AttachComponent(new BoundingBox() { IsDynamic = false, Width = w, Height = h });
            WriteEntity(file, serializeOptions, wall);
        }

        private static void WriteEntity(System.IO.StreamWriter file, JsonSerializerOptions serializeOptions, Entity entity) // TODO move somewhere else
        {
            file.WriteLine("!---");
            foreach (var component in entity.GetAllComponents())
            {
                file.WriteLine(component.GetType());
                file.WriteLine(JsonSerializer.Serialize(component, component.GetType(), serializeOptions));
            }
            file.WriteLine("---!");
        }

        public static void PaintTile(int layer, int x, int y, TileType type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
        }

        public static void FillTile(int layer, int x, int y, TileType type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            var currentTile = _map.GetTileType(layer, x, y);
            if (currentTile == type)
            {
                return;
            }

            FillRecursive(layer, x, y, type, currentTile);
        }

        private static void FillRecursive(int layer, int x, int y, TileType type, TileType toFill)
        {
            var currentTileType = _map.GetTileType(layer, x, y);
            if (x < 0 || x >= GetWidth() || y < 0 || y >= GetHeight() || currentTileType != toFill || currentTileType == type)
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
            FillRecursive(layer, x - 1, y, type, toFill);
            FillRecursive(layer, x + 1, y, type, toFill);
            FillRecursive(layer, x, y - 1, type, toFill);
            FillRecursive(layer, x, y + 1, type, toFill);
        }

        public static void AddLayer()
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.AddLayer();
        }

        public static void RemoveLayer(int layer)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.RemoveLayer(layer);
        }

        public static int GetTileDimension()
        {
            if (!MapIsLoaded())
            {
                return 1;
            }

            return _map.TileDimension;
        }

        public static int GetWidth()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.Width;
        }

        public static int GetHeight()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.Height;
        }

        public static TileType GetTileType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return TileType.None;
            }

            return _map.GetTileType(layer, x, y);
        }

        public static TransitionType GetTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return TransitionType.None;
            }

            return _map.GetTransitionType(layer, x, y);
        }

        public static PathDirection GetPathDirection(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return PathDirection.None;
            }

            return _map.GetPathDirection(layer, x, y);
        }

        public static int GetNumberOfLayers()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.layers.Count;
        }

    }
}
