using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LeyStoneEngine.Interface
{
    public interface IMovable
    {
        Vector2 velocity { get; set; }

        Vector2 Move();
    }
}
