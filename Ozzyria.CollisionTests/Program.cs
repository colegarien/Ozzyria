using Ozzyria.Game;
using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;
using System.Numerics;

namespace Ozzyria.CollisionTests
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) =>
            {
                ((Window)sender).Close();
            };

            var controlled = new Entity();
            controlled.AttachComponent(new Movement() { X = 100, Y = 100, PreviousX = 100, PreviousY = 100 });
            controlled.AttachComponent(new BoundingBox() { Width = 20, Height = 20 });


            var box1 = new Entity();
            box1.AttachComponent(new Movement() { X = 200, Y = 200, PreviousX = 200, PreviousY = 200 });
            box1.AttachComponent(new BoundingBox() { Width = 20, Height = 20 });

            var circle1 = new Entity();
            circle1.AttachComponent(new Movement() { X = 300, Y = 300, PreviousX = 300, PreviousY = 300 });
            circle1.AttachComponent(new BoundingCircle() { Radius = 10 });

            var swapDelay = 200f;
            var swapTimer = 0f;

            Stopwatch stopWatch = new Stopwatch();
            var deltaTime = 0f;
            while (window.IsOpen)
            {
                deltaTime = stopWatch.ElapsedMilliseconds;
                stopWatch.Restart();

                ///
                /// EVENT HANDLING HERE
                ///
                window.DispatchEvents();
                var quit = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Escape);
                var input = new Input
                {
                    MoveUp = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.W),
                    MoveDown = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.S),
                    MoveLeft = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.A),
                    MoveRight = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.D),
                    TurnLeft = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Q),
                    TurnRight = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.E),
                    Attack = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Space)
                };

                // UPDATE STUFF
                var speed = 0.04f;
                var velocityX = input.MoveLeft ? -speed : (input.MoveRight ? speed : 0);
                var velocityY = input.MoveUp ? -speed : (input.MoveDown ? speed : 0);

                var movement = controlled.GetComponent<Movement>(ComponentType.Movement);
                if (input.TurnLeft) {
                    movement.PreviousX += velocityX;
                    movement.PreviousY += velocityY;
                }
                else {
                    movement.X += velocityX;
                    movement.Y += velocityY;
                }

                swapTimer += deltaTime;
                if (input.Attack && swapTimer >= swapDelay)
                {
                    swapTimer = 0;
                    if(controlled.GetComponent<Collision>(ComponentType.Collision) is BoundingBox)
                    {
                        controlled.AttachComponent(new BoundingCircle() { Radius = 10 });
                    }
                    else
                    {
                        controlled.AttachComponent(new BoundingBox() { Width = 20, Height = 20 });
                    }
                }

                var collision1 = Collide(controlled, box1);
                var collision2 = Collide(controlled, circle1);

                // DRAW STUFF
                window.Clear();
                DrawEntity(window, box1);
                DrawEntity(window, circle1);
                DrawEntity(window, controlled);
                DrawCollisionResult(window, collision1, controlled, box1);
                DrawCollisionResult(window, collision2, controlled, circle1);
                window.Display();

                if (quit)
                {
                    window.Close();
                }
            }
        }

        private static void DrawEntity(RenderWindow window, Entity entity)
        {
            var movement = entity.GetComponent<Movement>(ComponentType.Movement);
            var collision = entity.GetComponent<Collision>(ComponentType.Collision);

            Shape shape;
            if(collision is BoundingCircle)
            {
                var circle = (BoundingCircle)collision;
                DrawShape(window, true, movement.PreviousX, movement.PreviousY, circle.Radius, circle.Radius, Color.Magenta);
                DrawShape(window, true, movement.X, movement.Y, circle.Radius, circle.Radius, Color.Red);
            }
            else
            {
                var box = (BoundingBox)collision;
                DrawShape(window, false, movement.PreviousX, movement.PreviousY, box.Width, box.Height, Color.Magenta);
                DrawShape(window, false, movement.X, movement.Y, box.Width, box.Height, Color.Red);
            }
        }

        private static void DrawShape(RenderWindow window, bool isCircle, float x, float y, float w, float h, Color color)
        {
            Shape shape;
            if (isCircle)
            {
                shape = new CircleShape(w);
                shape.Position = new SFML.System.Vector2f(x - w, y - w);
            }
            else
            {
                shape = new RectangleShape(new SFML.System.Vector2f(w, h));
                shape.Position = new SFML.System.Vector2f(x - (w / 2f), y - (h / 2f));
            }

            shape.FillColor = Color.Transparent;
            shape.OutlineColor = color;
            shape.OutlineThickness = 2;
            window.Draw(shape);
        }

        private static CollisionResult Collide(Entity entity1, Entity entity2)
        {
            var collision = entity1.GetComponent<Collision>(ComponentType.Collision);
            var otherCollision = entity2.GetComponent<Collision>(ComponentType.Collision);
            if (collision is BoundingCircle && otherCollision is BoundingCircle)
            {
                return Collision.CircleIntersectsCircle((BoundingCircle)collision, (BoundingCircle)otherCollision);
            }
            else if (collision is BoundingBox && otherCollision is BoundingBox)
            {
                return Collision.BoxIntersectsBox((BoundingBox)collision, (BoundingBox)otherCollision);
            }
            else if (collision is BoundingCircle && otherCollision is BoundingBox)
            {
                return Collision.CircleIntersectsBox((BoundingCircle)collision, (BoundingBox)otherCollision);
            }
            else if (collision is BoundingBox && otherCollision is BoundingCircle)
            {
                return Collision.BoxIntersectsCircle((BoundingBox)collision, (BoundingCircle)otherCollision);
            }

            return new CollisionResult() { Collided = false, NormalX = 0, NormalY = 0 };
        }

        private static void DrawCollisionResult(RenderWindow window, CollisionResult result, Entity entity1, Entity entity2)
        {
            var movement1 = entity1.GetComponent<Movement>(ComponentType.Movement);
            var movement2 = entity2.GetComponent<Movement>(ComponentType.Movement);
            if (result.Collided)
            {
                var dx = movement1.X - movement1.PreviousX;
                var dy = movement1.Y - movement1.PreviousY;
                // TODO play around with using dotProduct to correct current velocity (move direction + speed)
                var dotProduct = new Vector2(result.NormalX, result.NormalY) * Vector2.Dot(new Vector2(dx, dy), new Vector2(result.NormalX, result.NormalY));
                // This is for resolving intesecting
                var depthVector = new Vector2(result.NormalX, result.NormalY) * System.Math.Abs(result.Depth);

                DrawLegs(window, movement1, movement2);
                var verts = new Vertex[]
                {
                    //new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Green),
                    //new Vertex(new SFML.System.Vector2f(movement2.X-(dx - dotProduct.X), movement2.Y-(dy - dotProduct.Y)), Color.Green),
                    //new Vertex(new SFML.System.Vector2f(movement1.PreviousX, movement1.PreviousY), Color.Green),
                    //new Vertex(new SFML.System.Vector2f(movement1.PreviousX+(dx - dotProduct.X), movement1.PreviousY+(dy - dotProduct.Y)), Color.Green),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Yellow),
                    new Vertex(new SFML.System.Vector2f(movement2.X+(depthVector.X), movement2.Y+(depthVector.Y)), Color.Yellow),
                    new Vertex(new SFML.System.Vector2f(movement1.X, movement1.Y), Color.Green),
                    new Vertex(new SFML.System.Vector2f(movement1.X-(dotProduct.X), movement1.Y-(dotProduct.Y)), Color.Green),
                };
                window.Draw(verts, PrimitiveType.Lines);

                var collision = entity1.GetComponent<Collision>(ComponentType.Collision);
                var isCircle = collision is BoundingCircle;
                var width = isCircle ? ((BoundingCircle)collision).Radius : ((BoundingBox)collision).Width;
                var height = isCircle ? ((BoundingCircle)collision).Radius : ((BoundingBox)collision).Height;
                //DrawShape(window, entity1.GetComponent<Collision>(ComponentType.Collision) is BoundingCircle, movement1.PreviousX + (dx - dotProduct.X), movement1.PreviousY + (dy - dotProduct.Y), width, height, Color.Yellow);
                DrawShape(window, entity1.GetComponent<Collision>(ComponentType.Collision) is BoundingCircle, movement1.X + depthVector.X, movement1.Y + depthVector.Y, width, height, Color.Yellow);
                //movement.X = movement.PreviousX + (dx - dotProduct.X);
                //movement.Y = movement.PreviousY + (dy - dotProduct.Y);
            }
        }

        private static void DrawLegs(RenderWindow window, Movement movement1, Movement movement2)
        {
            var verts = new Vertex[]
            {
                    new Vertex(new SFML.System.Vector2f(movement1.X, movement1.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement1.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.X, movement1.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.X, movement1.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement1.Y), Color.Blue),


                    new Vertex(new SFML.System.Vector2f(movement1.PreviousX, movement1.PreviousY), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.PreviousX, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement1.PreviousY), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.PreviousX, movement1.PreviousY), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.PreviousX, movement2.Y), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement1.PreviousX, movement1.PreviousY), Color.Blue),
                    new Vertex(new SFML.System.Vector2f(movement2.X, movement1.PreviousY), Color.Blue),
            };

            window.Draw(verts, PrimitiveType.Lines);
        }
    }
}
