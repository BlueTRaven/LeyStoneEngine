using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LeyStoneEngine;
using LeyStoneEngine.Utility;
using LeyStoneEngine.Graphics;

namespace LeyStoneEngine.Entities
{
    public class SeperatingNode
    {
        public Vector2 node1, node2;
        public readonly Vector2 perp;

        public Timer timer;
        public SeperatingNode(Vector2 position, Vector2 perp, int nodeDuration)
        {
            this.node1 = this.node2 = position;

            this.perp = Vector2.Normalize(perp);   //perp should be determined before based on velocity.

            this.timer = Timer.CreateTimer(nodeDuration, BaseWorld.timeScale);
        }

        public void Seperate(float speed)
        {
            node1 += perp * speed;
            node2 += -perp * speed;
        }
    }

    public class VisualTrail : Visual
    {
        Timer nodeTimer;
        int nodeDuration;

        private List<SeperatingNode> nodes = new List<SeperatingNode>();

        private List<PrimitivePolygon> polygons = new List<PrimitivePolygon>();

        private readonly Color color;
        private Color fadeColor;

        private float expandRate = .25f;

        public VisualTrail(Vector2 position, int time, int nodeDuration, float expandRate, Color color) : base(position, 4)
        {
            nodeTimer = Timer.CreateTimer(time, BaseWorld.timeScale, true, true);
            nodeTimer.time = 0; //set this to 0, since the full time is already recorded in the timer's constructor. This forces it to create a node instantly.

            this.nodeDuration = nodeDuration;

            this.expandRate = expandRate;

            this.color = color;

            this.solid = false;
        }

        public override void Update(BaseWorld world)
        {
            base.Update(world);

            if (nodeTimer.time <= 0)
            {
                nodes.Add(new SeperatingNode(position, VectorHelper.GetPerpendicular(velocity), nodeDuration));
            }

            foreach (SeperatingNode n in nodes.ToList())
            {
                if (!n.timer.done)
                    n.Seperate(expandRate * BaseWorld.timeScale.scale);
                else nodes.Remove(n);
            }

            if (nodes.Count > 1)
            {   //at least 2 nodes
                polygons.Clear();
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (i + 1 < nodes.Count)   //make sure we have a node to pair it with. i will always be < nodes.Count - 1, since count is not 0 indexed.
                    {
                        //if (i < nodes.Count / 2)
                            polygons.Add(new PrimitivePolygon(
                            new Vector2[] { nodes[i].node1, nodes[i].node2, nodes[i + 1].node2, nodes[i + 1].node1 },
                            new Color[] { Color.Lerp(Color.Transparent, color, nodes[i].timer.time / nodes[i].timer.setTime), Color.Lerp(Color.Transparent, color, nodes[i].timer.time / nodes[i].timer.setTime), Color.Lerp(Color.Transparent, color, nodes[i + 1].timer.time / nodes[i + 1].timer.setTime), Color.Lerp(Color.Transparent, color, nodes[i + 1].timer.time / nodes[i + 1].timer.setTime) }));
                        /*else
                            polygons.Add(new PrimitivePolygon(
                            new Vector2[] { nodes[i].node1, nodes[i].node2, nodes[i + 1].node2, nodes[i + 1].node1 },
                            new Color[] { color, color, color, color }));*/

                    }
                }
                Console.WriteLine(polygons.Count);
            }
        }

        public void ClearNodes()
        {
            nodes.Clear();
            polygons.Clear();
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (PrimitivePolygon poly in polygons)
            {
                poly.Draw(batch, new EffectInstance[] { new EffectInstance(BaseMain.assets.GetEffectContainer("basicEffect"), new Action<EffectInstance>((instance) => 
                {
                    //((BasicEffect)instance.effectContainer.effect).World = BaseMain.camera.GetViewMatrix();
                }))
                //, new EffectInstance(BaseMain.assets.GetEffectContainer("sharpen"))
                }.ToList());
            }
        }
    }
}
