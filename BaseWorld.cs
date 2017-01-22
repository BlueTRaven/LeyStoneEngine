using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LeyStoneEngine.Collision;
using LeyStoneEngine.Utility;
using LeyStoneEngine.Interface;
using LeyStoneEngine.Entities;
using LeyStoneEngine.Graphics;
using LeyStoneEngine.Serialization;
using LeyStoneEngine.Triggers;

using Google.Protobuf;

namespace LeyStoneEngine
{
    public enum RequiredSelectionMode
    {
        Texture,
        Collision,
        Entity
    }

    public abstract class BaseWorld
    {
        public readonly Vector2 size;

        public List<Background> backgrounds = new List<Background>(8);  //Limited to 8 backgrounds. TODO: Serialized.

        public List<PrimitivePolygon> polygons = new List<PrimitivePolygon>(512);      //Limited to 512 triangles. TODO: Serialized.

        public List<Line> lines = new List<Line>(256);                  //Limited to 256 collision lines. Serialized.

        public List<Trigger> triggers = new List<Trigger>(128);         //Limited to 128 triggers. Serialized.

        public List<Entity> entities = new List<Entity>(256);           //Limited to 256 entities. Serialized.

        public Rectangle bounds;

        public readonly float gravity = .5f;

        protected int counter;

        public int index;

        public static TimeScale timeScale;

        private bool started;

        public BaseWorld(Vector2 size)
        {
            timeScale = new TimeScale(0);
            bounds = new Rectangle(Point.Zero, size.ToPoint());
            this.size = size;
        }

        public void Start()
        {
            started = true;
            timeScale.scale = 1;
        }

        public virtual void Update(BaseMain main)
        {
            if (!started)
                throw new Exception("Start function not called!");
            counter++;

            foreach (Entity e in entities.ToList())
            {
                e.Update(this);

                if (e.dead)
                    entities.Remove(e);
            }

            foreach (Trigger t in triggers.ToList())
            {
                t.CheckTrigger(this);

                if (t.dead)
                    triggers.Remove(t);
            }
        }

        public abstract void Draw(GraphicsDevice device, SpriteBatch batch);

        public void Serialize(string output)
        {
            string fout = BaseMain.saveDirectory + output;

            SerVector serSize = new SerVector
            {
                X = size.X,
                Y = size.Y
            };

            List<SerLine> serlines = new List<SerLine>();
            List<SerEntity> serentities = new List<SerEntity>();
            List<SerPrimPolygon> serpolygons = new List<SerPrimPolygon>();
            List<SerTrigger> sertriggers = new List<SerTrigger>();

            foreach (Line l in lines)
            {
                serlines.Add(new SerLine
                {
                    LeftNode = new SerVector
                    {
                        X = l.leftNode.position.X,
                        Y = l.leftNode.position.Y
                    },

                    RightNode = new SerVector
                    {
                        X = l.rightNode.position.X,
                        Y = l.rightNode.position.Y
                    }
                });
            }

            foreach (Entity e in entities)
            {
                serentities.Add(new SerEntity
                {
                    Type = e.entityType,

                    Position = new SerVector
                    {
                        X = e.position.X,
                        Y = e.position.Y
                    }
                });
            }

            foreach (PrimitivePolygon poly in polygons)
            {

                List<SerPrimTriangle> tris = new List<SerPrimTriangle>();
                foreach (PrimitiveTriangle triangle in poly.triangles)
                {   
                    tris.Add(new SerPrimTriangle
                    {
                        Point1 = new Vector2(triangle.vertices[0].Position.X, triangle.vertices[0].Position.Y).ToSerVector(),
                        Point2 = new Vector2(triangle.vertices[1].Position.X, triangle.vertices[1].Position.Y).ToSerVector(),
                        Point3 = new Vector2(triangle.vertices[2].Position.X, triangle.vertices[2].Position.Y).ToSerVector(),

                        Color1 = triangle.vertices[0].Color.ToSerColor(),
                        Color2 = triangle.vertices[1].Color.ToSerColor(),
                        Color3 = triangle.vertices[2].Color.ToSerColor(),

                        TextureCoord1 = triangle.vertices[0].TextureCoordinate.ToSerVector(),
                        TextureCoord2 = triangle.vertices[1].TextureCoordinate.ToSerVector(),
                        TextureCoord3 = triangle.vertices[2].TextureCoordinate.ToSerVector(),
                    });
                }

                serpolygons.Add(new SerPrimPolygon
                {
                    Triangles = { tris }
                });
            }

            foreach (Trigger trig in triggers)
            {
                sertriggers.Add(new SerTrigger
                {
                    Position = trig.bounds.Location.ToVector2().ToSerVector(),
                    Size = trig.bounds.Size.ToVector2().ToSerVector(),
                    Type = trig.triggerType,
                    TriggeredByType = trig.triggeredByType
                });
            }

            SerWorld serworld = new SerWorld
            {
                Lines = { serlines },
                Entities = { serentities },
                Polygons = { serpolygons },

                Size = new SerVector
                {
                    X = size.X,
                    Y = size.Y
                }
            };

            using (var outp = File.Create(fout))
            {
                serworld.WriteTo(outp);
            }
        }

        public virtual void Deserialize(string input)
        {
            string finp = BaseMain.saveDirectory + input;

            lines.Clear();
            entities.Clear();
            polygons.Clear();
            triggers.Clear();
            //player = null;

            SerWorld world;

            using (var inp = File.OpenRead(finp))
            {
                world = SerWorld.Parser.ParseFrom(inp);
            }

            index = world.Index;

            List<SerEntity> ents = world.Entities.ToList();
            List<SerLine> lines2 = world.Lines.ToList();
            List<SerPrimPolygon> polys = world.Polygons.ToList();

            foreach (SerEntity e in ents)
            {
                entities.Add(GetEntityFromType(e.Type, e.SubType, e.Position.ToVector2()));
            }

            foreach (SerLine l in lines2)
            {
                lines.Add(new Line(new Node(l.RightNode.ToVector2() /*new Vector2(l.RightNode.X, l.RightNode.Y)*/), new Node(l.LeftNode.ToVector2()/*new Node(new Vector2(l.LeftNode.X, l.LeftNode.Y)*/)));
            }

            foreach (SerPrimPolygon p in polys)
            {
                polygons.Add(p.ToPolygon());
                /*List<Vector2> verts = new List<Vector2>();
                List<Color> colors = new List<Color>();
                foreach (SerVector sv in p.Vertices)
                {
                    verts.Add(sv.ToVector2());
                }

                foreach (SerColor sc in p.Colors)
                {
                    colors.Add(sc.ToColor());
                }

                polygons.Add(new PrimitivePolygon(verts.ToArray(), colors.ToArray()));*/
            }
        }

        public BaseWorld CheckConvertWorld(object d)
        {
            if (typeof(BaseWorld).IsAssignableFrom(d.GetType()))
                return (BaseWorld)d;
            else throw new Exception("Could not convert object of type " + d.GetType() + " To BaseWorld type.");
        }
        public abstract Entity GetEntityFromType(int type, int subType, Vector2 position, params int[] additionalInformation);
    }
}
