using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame2
{
    class GameObject
    {
        public Texture2D texture;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public float speed;

        public GameObject(Texture2D loadedTexture)
        {
            texture = loadedTexture;
            rotation = 0.0f;
            position = Vector2.Zero;
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
        }

    }
}
