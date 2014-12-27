using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace WindowsGame2
{
    class MenuScreen
    {
        string gameName = "Yeti UFO Defense";
        private Texture2D background;
        SpriteFont spriteFont;
        int selectedIndex = 0;
        private string[] menuItems = new string[] {"Play", "High Scores", "Credits"};
        KeyboardState previousKeyboardState;
        GamePadState previousGamePadState;

        public MenuScreen(Texture2D texture, SpriteFont font)
        {
            background = texture;
            spriteFont = font;
        }

        public void Update()
        {
#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up) &&
                previousKeyboardState.IsKeyUp(Keys.Up))
            {
                selectedIndex--;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) &&
                previousKeyboardState.IsKeyUp(Keys.Down))
            {
                selectedIndex++;
            }

            if (selectedIndex < 0)
            {
                selectedIndex = menuItems.Length - 1;
            }
            if (selectedIndex > menuItems.Length - 1)
            {
                selectedIndex = 0;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) &&
                previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        {
                            Game1.currentGameState = Game1.GameState.GAME;
                            break;
                        }
                    case 1:
                        {
                            Game1.currentGameState = Game1.GameState.HIGH_SCORES;
                            break;
                        }
                    case 2:
                        {
                            Game1.currentGameState = Game1.GameState.CREDITS;
                            break;
                        }
                }
            }

            previousKeyboardState = keyboardState;
#endif
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gamePadState.IsButtonDown(Buttons.DPadUp) &&
                previousGamePadState.IsButtonUp(Buttons.DPadUp))
            {
                selectedIndex--;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadDown) &&
                previousGamePadState.IsButtonUp(Buttons.DPadDown))
            {
                selectedIndex++;
            }

            if (selectedIndex < 0)
            {
                selectedIndex = menuItems.Length - 1;
            }
            if (selectedIndex > menuItems.Length - 1)
            {
                selectedIndex = 0;
            }

            if (gamePadState.IsButtonDown(Buttons.A) &&
                previousGamePadState.IsButtonDown(Buttons.A))
            {
                switch (selectedIndex)
                {
                    case 0:
                        {
                            Game1.currentGameState = Game1.GameState.GAME;
                            break;
                        }
                    case 1:
                        {
                            Game1.currentGameState = Game1.GameState.HIGH_SCORES;
                            break;
                        }
                    case 2:
                        {
                            Game1.currentGameState = Game1.GameState.CREDITS;
                            break;
                        }
                }
            }

            previousGamePadState = gamePadState;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle viewportRectangle)
        {
            spriteBatch.Draw(background, viewportRectangle, Color.White);
            Vector2 screenCenter = new Vector2((float)viewportRectangle.Width / 2,
                (float)viewportRectangle.Height / 2);

            Vector2 textSize = spriteFont.MeasureString(gameName);
            Vector2 namePosition = new Vector2(screenCenter.X - (textSize.X / 2), 20);
            spriteBatch.DrawString(spriteFont, gameName, namePosition, Color.White);
            Vector2 position = new Vector2(120, 80);
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    spriteBatch.DrawString(spriteFont, menuItems[i], position, Color.White);
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, menuItems[i], position, Color.Red);
                }
                position.Y += 40;
            }
        }

    }
}
