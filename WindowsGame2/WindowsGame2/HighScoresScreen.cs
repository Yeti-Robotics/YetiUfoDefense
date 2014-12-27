using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using System.Collections;

namespace WindowsGame2
{
    class HighScoresScreen
    {
        [Serializable]
        public struct HighScoreData : IComparable<HighScoreData>
        {
            public string name;
            public int score;

            public HighScoreData(string playerName, int playerScore)
            {
                name = playerName;
                score = playerScore;
            }

            public int CompareTo(HighScoreData other)
            {
                if (score > other.score)
                {
                    return -1;
                }
                else if (score < other.score)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private Texture2D background;
        SpriteFont spriteFont;
        KeyboardState previousKeyboardState;
        GamePadState previousGamePadState;
        List<HighScoreData> scores;
        Vector2 drawPosition;
        bool enterHighScoresMode = false;
        char[] playerName;
        const char MIN_LETTER = 'A';
        const char MAX_LETTER = 'Z';
        SpriteFont scoreFont;
        int score = 200;
        int selectedLetter = 0;
        public int scroll = 0;

        public HighScoresScreen(Texture2D texture, SpriteFont font, SpriteFont font2)
        {
            background = texture;
            spriteFont = font;
            scores = new List<HighScoreData>();

            drawPosition = new Vector2(80, 80);
            scoreFont = font2;
            playerName = new char[] { MIN_LETTER, MIN_LETTER, MIN_LETTER};
        }

        public void EnterScoreMode(int playerScore)
        {
            selectedLetter = 0;
            scroll = 0;
            score = playerScore;
            enterHighScoresMode = true;
            playerName = new char[] { MIN_LETTER, MIN_LETTER, MIN_LETTER };
        }

        public void ViewHighScoreMode()
        {
            enterHighScoresMode = false;
        }

        public void Update()
        {
            if (enterHighScoresMode)
            {
                EditName();
            }
            else
            {
                UpdateViewHighScores();
            }
        }

        private void UpdateViewHighScores()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Up) || 
                gamepadState.IsButtonDown(Buttons.DPadUp))
            {
                scroll += 5;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) ||
                gamepadState.IsButtonDown(Buttons.DPadDown))
            {
                scroll += -5;
            }
        }

        private void EditName()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if ((keyboardState.IsKeyDown(Keys.Up) &&
                previousKeyboardState.IsKeyUp(Keys.Up)) ||
                (gamepadState.IsButtonDown(Buttons.DPadUp) &&
                previousGamePadState.IsButtonUp(Buttons.DPadUp)))
            {
                playerName[selectedLetter]--;
            }
            else if ((keyboardState.IsKeyDown(Keys.Down) &&
                previousKeyboardState.IsKeyUp(Keys.Down)) ||
                (gamepadState.IsButtonDown(Buttons.DPadDown) &&
                previousGamePadState.IsButtonUp(Buttons.DPadDown)))
            {
                playerName[selectedLetter]++;
            }

            if (playerName[selectedLetter] < MIN_LETTER)
            {
                playerName[selectedLetter] = MAX_LETTER;
            }
            else if (playerName[selectedLetter] > MAX_LETTER)
            {
                playerName[selectedLetter] = MIN_LETTER;
            }
            else if ((keyboardState.IsKeyDown(Keys.Enter) &&
                previousKeyboardState.IsKeyUp(Keys.Enter)) ||
                (gamepadState.IsButtonDown(Buttons.A) &&
                previousGamePadState.IsButtonUp(Buttons.A)))
            {
                selectedLetter++;
            }

            if (selectedLetter >= playerName.Length)
            {
                selectedLetter = 0;
                String name = "";
                foreach (char letter in playerName)
                {
                    name += letter;
                }
                AddNewHighScore(name, score);
                ViewHighScoreMode();
            }

            previousGamePadState = gamepadState;
            previousKeyboardState = keyboardState;
        }

        public void AddNewHighScore(string name, int score)
        {
            scores.Add(new HighScoreData(name, score));
            scores.Sort();
        }

        public void DrawViewHighScores(SpriteBatch spriteBatch, Rectangle viewportRectangle)
        {
            Vector2 screenCenter = new Vector2((float)viewportRectangle.Width / 2, (float)viewportRectangle.Height / 2);
            Vector2 textSize = spriteFont.MeasureString("W.  WWW   -----------------   WWW");
            screenCenter.X -= textSize.X / 2;
            screenCenter.Y -= textSize.Y / 2;

            spriteBatch.DrawString(spriteFont, "#.Name   -----------------   Score", 
                new Vector2(screenCenter.X, 30 + scroll), Color.Yellow);

            Vector2 tempDrawPosition = new Vector2(screenCenter.X, 60 + scroll);
            int count = 1;
            foreach (HighScoreData data in scores)
            {
                string text = count + ".  " + data.name + "   -----------------   " + data.score;
                spriteBatch.DrawString(spriteFont, text, tempDrawPosition, Color.Yellow);
                tempDrawPosition.Y += 30;
                count++;
            }
        }

        public void DrawEnterHighScores(SpriteBatch spriteBatch, Rectangle viewportRectangle)
        {
            string instructions = "Enter your initals. Your score is: " + score;
            Vector2 screenCenter = new Vector2((float)viewportRectangle.Width / 2, (float)viewportRectangle.Height / 2);
            Vector2 textSize = spriteFont.MeasureString(instructions);
            screenCenter.X -= textSize.X / 2;
            screenCenter.Y -= textSize.Y / 2;

            spriteBatch.DrawString(spriteFont, instructions, screenCenter, Color.Yellow);

            Vector2 tempDrawPosition = new Vector2(screenCenter.X, screenCenter.Y + 20);
            int count = 0;
            foreach (char letter in playerName)
            {
                if (selectedLetter == count)
                {
                    spriteBatch.DrawString(scoreFont, letter + "", tempDrawPosition, Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(scoreFont, letter + "", tempDrawPosition, Color.Yellow);
                }
                
                tempDrawPosition.X += 80;
                count++;
            }
            //spriteBatch.DrawString(scoreFont, score + "", tempDrawPosition, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle viewportRectangle)
        {
            spriteBatch.Draw(background, viewportRectangle, Color.White);
            if (enterHighScoresMode)
            {
                DrawEnterHighScores(spriteBatch, viewportRectangle);
            }
            else
            {
                DrawViewHighScores(spriteBatch, viewportRectangle);
            }
        }
    }
}