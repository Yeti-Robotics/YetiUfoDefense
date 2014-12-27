using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame2
{
    class CreditsScreen
    {
        private Texture2D background;
        SpriteFont spriteFont;
        string[] credits = {"Team Yeti 3506",
                           " ",
                           "Head Keyboard Masher",
                           "Sam u5hkgsexnmjk,l;[Point;iuhygtvfbiv",
                           " ",
                           "Master of Pressing Keys in an Encoded Manner",
                           "Matt Leonard",
                           " ",
                           "Lord of Art", 
                           "Sir John",
                           " ",
                           "Co-Artist",
                           "Taylor",
                           " ",
                           "Sir Lord Chief Executive and General of All That is Good and Wonderful and Also That One Guy Who Designed the Missile That No One Can See Because It's So Small and Moves At LUDICROUS SPEED",
                           "Noah",
                           " ",
                           "Guy who walks in occasionally",
                           "Mitch & Moby",
                           " ",
                           "Executive Sprocket",
                           "Antoine Campbell"
                           };

        public CreditsScreen(SpriteFont font, Texture2D texture)
        {
            spriteFont = font;
            background = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle viewportRectangle)
        {
            spriteBatch.Draw(background, viewportRectangle, Color.White);
            Vector2 screenCenter = new Vector2((float)viewportRectangle.Width / 2,
                (float)viewportRectangle.Height / 2);

            int offset = 5;
            foreach (string text in credits)
            {
                Vector2 textSize = spriteFont.MeasureString(text);
                Vector2 drawPosition = new Vector2(screenCenter.X - (textSize.X / 2), offset);
                spriteBatch.DrawString(spriteFont, text, drawPosition, Color.LimeGreen);
                offset += 24;
            }

            
        }
    }
}
