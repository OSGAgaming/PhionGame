using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Entities.EntitySystems;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.Tiles;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QueefCord.Content.Entities
{
    public struct EdgeCase
    {
        public Vector2 position;
        public float angle;

        public EdgeCase(Vector2 position, float angle)
        {
            this.position = position;
            this.angle = angle;
        }
    }
    public class LineOfSight : Entity, IUpdate, IDraw
    {
        public string Layer => "Default";

        public void Update(GameTime gameTime)
        {
            Transform.Position = Player.LocalPlayer.Center;

            List<Rectangle> rs = new List<Rectangle>();

            foreach (TileSet tileSet in TileManager.Instance.TileSets.Values)
            {
                if (tileSet.Solid)
                    rs = rs.Concat(tileSet.CollisionBoxes).ToList();
            }

            Point p1 = Point.Zero.ToVector2().ToDefaultScreen().ToPoint();
            Point p2 = Renderer.BackBufferSize;

            rs.Add(new Rectangle(p1, p2));

            EdgeCase[] cases = GetFanPoints(rs.ToArray()).OrderBy(n => n.angle).ToArray();

            LayerHost.GetLayer(Layer).MapHost.Maps.DrawToPrimitiveMap("LineOfSightMap",
                (SpriteBatch sb) =>
                {
                    foreach (TriMesh triMesh in GetMeshes(cases, Transform.Position))
                        triMesh.Draw(sb);
                });

            LayerHost.GetLayer(Layer).MapHost.Maps.DrawToBatchedMap("LightMap",
                (SpriteBatch sb) =>
                {
                    Texture2D tex = Assets<Texture2D>.Get("Textures/Maps/PointLight");
                    sb.Draw(tex, Transform.Position, Color.White * 0.4f, tex.TextureCenter());
                });
        }

        public TriMesh[] GetMeshes(EdgeCase[] edgeCases, Vector2 center)
        {
            List<TriMesh> meshes = new List<TriMesh>();

            int index = 0;
            foreach (EdgeCase c in edgeCases)
            {
                if (index > 0)
                    meshes.Add(new TriMesh(center, edgeCases[index].position, edgeCases[index - 1].position, Color.White * 0.6f, Layer));
                else
                    meshes.Add(new TriMesh(center, edgeCases[index].position, edgeCases.Last().position, Color.White * 0.6f, Layer));
                index++;
            }

            return meshes.ToArray();
        }

        public EdgeCase[] GetFanPoints(Rectangle[] rects)
        {
            List<EdgeCase> edgeCases = new List<EdgeCase>();

            foreach (Rectangle rect in rects)
            {
                Point[] points = new Point[4];

                points[0] = rect.Location;
                points[1] = rect.Location.Add(new Point(rect.Width, 0));
                points[2] = rect.Location.Add(new Point(0, rect.Height));
                points[3] = rect.Location.Add(new Point(rect.Width, rect.Height));

                foreach (Point p in points)
                {
                    Vector2 v = Utils.GetClosestPoint(rects, Transform.Position.ToPoint(), p);
                    Vector2 pos;
                    float angle;

                    if (v == new Vector2(int.MaxValue))
                    {
                        pos = p.ToVector2();
                    }
                    else
                        pos = v;

                    angle = (pos - Transform.Position).ToRotation();
                    edgeCases.Add(new EdgeCase(pos, angle));

                    Vector2 epsilonF = Transform.Position + Vector2.Normalize(pos - Transform.Position).RotatedBy(0.07f) * 10000;
                    Vector2 epsilonB = Transform.Position + Vector2.Normalize(pos - Transform.Position).RotatedBy(-0.07f) * 10000;

                    Vector2 v1 = Utils.GetClosestPoint(rects, Transform.Position.ToPoint(), epsilonF.ToPoint());
                    Vector2 v2 = Utils.GetClosestPoint(rects, Transform.Position.ToPoint(), epsilonB.ToPoint());

                    edgeCases.Add(new EdgeCase(v1, (v1 - Transform.Position).ToRotation()));
                    edgeCases.Add(new EdgeCase(v2, (v2 - Transform.Position).ToRotation()));
                }
            }

            return edgeCases.ToArray();
        }

        public void Draw(SpriteBatch sb)
        {
            Texture2D tex = LayerHost.GetLayer(Layer).MapHost.Maps.Get("LineOfSightMap").MapTarget;
            //sb.Draw(tex, new Rectangle(Point.Zero, Renderer.BackBufferSize), Color.White);
        }
    }
}
