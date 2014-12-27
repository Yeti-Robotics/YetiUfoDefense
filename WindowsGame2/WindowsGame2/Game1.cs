using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum GameState
        {
            GAME, MENU, HIGH_SCORES, CREDITS
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle viewportRectangle;
        Texture2D backgroundTexture;
        GameObject cannon;
        const int MAX_CANNON_BALLS = 3;
        GameObject[] cannonBalls;
        GamePadState previousGamePadState;
        KeyboardState previousKeyboardState;
        GameObject[] enemies;
        const int MAX_ENEMIES = 5;
        const float maxEnemyHeight = 0.1f;
        const float minEnemyHeight = 0.5f;
        const float maxEnemyVelocity = 10.0f;
        const float minEnemyVelocity = 5.0f;
        Random random = new Random();
        const float CANNONBALL_SPEED = 12.0f;
        SpriteFont spriteFont;
        int score = 0;
        GameObject[] missiles;
        const float MISSILE_SPEED = 20.0f;
        const int MAX_MISSILES = 1;
        MenuScreen menuScreen;
        public static GameState currentGameState = GameState.MENU;
        float health;
        const float MAX_HEALTH = 100f;
        Rectangle healthRectangle;
        Texture2D healthTexture;
        const int MAX_HEALTH_WIDTH = 280;
        const int HEALTH_HEIGHT = 20;
        const int HEALTH_PADDING_X = 40;
        const int HEALTH_PADDING_Y = 40;
        HighScoresScreen highScoresScreen;
        Texture2D healtBarOutline;
        Vector2 healthBarOutlinePosition;
        SpriteFont scoreFont;
        CreditsScreen creditsScreen;
        GameObject cannonBase;
        GameObject yeti;
        Color healthBarColor;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 640;
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            viewportRectangle = new Rectangle(0, 0, 
                graphics.GraphicsDevice.Viewport.Width, 
                graphics.GraphicsDevice.Viewport.Height);

            backgroundTexture = Content.Load<Texture2D>("background");
            cannon = new GameObject(Content.Load<Texture2D>("cannon_main"));
            cannon.position = new Vector2(120, 
                graphics.GraphicsDevice.Viewport.Height - 80);
            cannonBase = new GameObject(Content.Load<Texture2D>("cannon_base"));
            cannonBase.position = cannon.position;

            cannonBalls = new GameObject[MAX_CANNON_BALLS];
            for (int i = 0; i < MAX_CANNON_BALLS; i++)
            {
                cannonBalls[i] = new GameObject(Content.Load<Texture2D>("cannonball"));
            }

            enemies = new GameObject[MAX_ENEMIES];
            for (int i = 0; i < MAX_ENEMIES; i++)
            {
                enemies[i] = new GameObject(Content.Load<Texture2D>("enemy2"));
            }

            spriteFont = Content.Load<SpriteFont>("GameFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            missiles = new GameObject[MAX_MISSILES];
            for (int i = 0; i < MAX_MISSILES; i++)
            {
                missiles[i] = new GameObject(Content.Load<Texture2D>("missile2"));
            }
            menuScreen = new MenuScreen(backgroundTexture, spriteFont);
            highScoresScreen = new HighScoresScreen(backgroundTexture, spriteFont, scoreFont);
            creditsScreen = new CreditsScreen(spriteFont, backgroundTexture);

            health = MAX_HEALTH;
            healthRectangle = new Rectangle(viewportRectangle.Width - MAX_HEALTH_WIDTH - HEALTH_PADDING_X,
                HEALTH_PADDING_Y, MAX_HEALTH_WIDTH, HEALTH_HEIGHT);
            healthTexture = Content.Load<Texture2D>("dot");

            healtBarOutline = Content.Load<Texture2D>("healthbar");
            healthBarOutlinePosition = new Vector2(healthRectangle.Left - 7,
                healthRectangle.Top - 13);

            yeti = new GameObject(Content.Load<Texture2D>("yeti"));
            yeti.position = new Vector2(cannonBase.position.X - 30,
                cannonBase.position.Y);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void UpdateGame()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            cannon.rotation += gamePadState.ThumbSticks.Left.X * 0.1f;
            //cannon.position.Y -= gamePadState.ThumbSticks.Left.Y * 10.0f;
            //cannon.position.X += gamePadState.ThumbSticks.Left.X * 10.0f;
            //cannon.rotation += gamePadState.ThumbSticks.Right.X * 0.1f;
            cannon.rotation = MathHelper.Clamp(cannon.rotation, -MathHelper.PiOver2, 0);

            if (gamePadState.Buttons.A == ButtonState.Pressed &&
                previousGamePadState.Buttons.A == ButtonState.Released)
            {
                FireCannonBall();
            }
            else if (gamePadState.Buttons.B == ButtonState.Pressed &&
                previousGamePadState.Buttons.B == ButtonState.Released)
            {
                FireMissile();
            }

#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                cannon.rotation += -0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                cannon.rotation += 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.Space) &&
                previousKeyboardState.IsKeyUp(Keys.Space))
            {
                FireCannonBall();
            }
            else if (keyboardState.IsKeyDown(Keys.LeftControl) &&
                previousKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                FireMissile();
            }

#endif

            UpdateCannonBalls();
            UpdateMissiles();
            UpdateEnemies();

            healthRectangle.Width = (int) ((health / MAX_HEALTH) * MAX_HEALTH_WIDTH);
            if (health <= 0)
            {
                EndGame();
            }

            if (health >= (MAX_HEALTH * 0.75f))
            {
                healthBarColor = Color.Green;
            }
            else if (health >= (MAX_HEALTH * 0.5f))
            {
                healthBarColor = Color.Yellow;
            }
            else if (health >= (MAX_HEALTH * 0.25f))
            {
                healthBarColor = Color.Orange;
            }
            else if (health < (MAX_HEALTH * 0.25f))
            {
                healthBarColor = Color.Red;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if ((previousGamePadState.Buttons.Back == ButtonState.Released && 
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                (previousKeyboardState.IsKeyUp(Keys.Escape) &&
                Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                if (currentGameState == GameState.GAME)
                {
                    currentGameState = GameState.MENU;
                    ResetGame();
                }
                else if (currentGameState == GameState.HIGH_SCORES)
                {
                    currentGameState = GameState.MENU;
                    highScoresScreen.scroll = 0;
                }
                else if (currentGameState == GameState.MENU)
                {
                    this.Exit();
                }
                else if (currentGameState == GameState.CREDITS)
                {
                    currentGameState = GameState.MENU;
                }
            }

            switch (currentGameState)
            {
                case GameState.GAME:
                    {
                        UpdateGame();
                        break;
                    }
                case GameState.MENU:
                    {
                        menuScreen.Update();
                        break;
                    }
                case GameState.HIGH_SCORES:
                    {
                        highScoresScreen.Update();
                        break;
                    }
            }

            previousGamePadState = GamePad.GetState(PlayerIndex.One);
#if !XBOX
            previousKeyboardState = Keyboard.GetState();
#endif
            
            base.Update(gameTime);
        }

        public void UpdateEnemies()
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    enemy.position += enemy.velocity;
                    if (!viewportRectangle.Contains(
                        new Point((int)enemy.position.X, (int)enemy.position.Y)))
                    {
                        enemy.alive = false;
                        health--;
                    }
                }
                else
                {
                    enemy.alive = true;

                    float randomValue = (float)random.NextDouble();
                    float randomHeight = MathHelper.Lerp(minEnemyHeight * viewportRectangle.Height,
                        maxEnemyHeight * viewportRectangle.Height, 
                        randomValue);
                    enemy.position = new Vector2(viewportRectangle.Width, randomHeight);

                    randomValue = (float)random.NextDouble();
                    float enemySpeed = MathHelper.Lerp(minEnemyVelocity,
                        maxEnemyVelocity,
                        randomValue);
                    enemy.velocity = new Vector2(-enemySpeed, 0);
                }
            }
        }

        public void UpdateCannonBalls()
        {
            foreach (GameObject ball in cannonBalls)
            {
                if (ball.alive)
                {
                    ball.position += ball.velocity;
                    if (!viewportRectangle.Contains(new Point(
                        (int)ball.position.X,
                        (int)ball.position.Y)))
                    {
                        ball.alive = false;
                        continue;
                    }
                    Rectangle cannonballRect = new Rectangle((int)ball.position.X,
                        (int)ball.position.Y,
                        ball.texture.Width,
                        ball.texture.Height);
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            Rectangle enemyRect = new Rectangle((int)enemy.position.X,
                                (int)enemy.position.Y,
                                enemy.texture.Width,
                                enemy.texture.Height);
                            if (cannonballRect.Intersects(enemyRect))
                            {
                                ball.alive = false;
                                enemy.alive = false;
                                score++;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void FireCannonBall()
        {
            foreach (GameObject ball in cannonBalls)
            {
                if (!ball.alive)
                {
                    ball.alive = true;
                    ball.position = cannon.position - ball.center;
                    ball.velocity = new Vector2((float)Math.Cos(cannon.rotation),
                        (float)Math.Sin(cannon.rotation)) * CANNONBALL_SPEED;
                    ball.position += new Vector2((float)Math.Cos(cannon.rotation),
                        (float)Math.Sin(cannon.rotation)) * 80.0f;
                    return;
                }
            }
        }

        public void FireMissile()
        {
            foreach (GameObject missile in missiles)
            {
                if (!missile.alive)
                {
                    missile.alive = true;
                    missile.position = cannon.position;
                    missile.rotation = cannon.rotation;
                    missile.velocity = new Vector2((float)Math.Cos(cannon.rotation),
                        (float)Math.Sin(cannon.rotation)) * MISSILE_SPEED;
                    return;
                }
            }
        }

        public void UpdateMissiles()
        {
            foreach (GameObject missile in missiles)
            {
                if (missile.alive)
                {
                    missile.position += missile.velocity;
                    if (!viewportRectangle.Contains(new Point(
                        (int)missile.position.X,
                        (int)missile.position.Y)))
                    {
                        missile.alive = false;
                        continue;
                    }
                    Rectangle missileRect = new Rectangle((int)missile.position.X,
                        (int)missile.position.Y,
                        missile.texture.Width,
                        missile.texture.Height);
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            Rectangle enemyRect = new Rectangle((int)enemy.position.X,
                                (int)enemy.position.Y,
                                enemy.texture.Width,
                                enemy.texture.Height);
                            if (missileRect.Intersects(enemyRect))
                            {
                                missile.alive = false;
                                enemy.alive = false;
                                score++;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void DrawGame(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, viewportRectangle, Color.White);

            foreach (GameObject ball in cannonBalls)
            {
                if (ball.alive)
                {
                    spriteBatch.Draw(ball.texture, ball.position, Color.White);
                }
            }

            foreach (GameObject missile in missiles)
            {
                if (missile.alive)
                {
                    spriteBatch.Draw(missile.texture,
                        missile.position,
                        null,
                        Color.White,
                        missile.rotation,
                        missile.center,
                        1.0f,
                        SpriteEffects.None,
                        0);
                }
            }

            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.texture, enemy.position, Color.White);
                }
            }

            spriteBatch.Draw(yeti.texture,
                yeti.position,
                null,
                Color.White,
                yeti.rotation,
                yeti.center,
                1.0f,
                SpriteEffects.None,
                0);
            spriteBatch.Draw(cannon.texture,
                cannon.position,
                null,
                Color.White,
                cannon.rotation,
                cannon.center,
                1.0f,
                SpriteEffects.None,
                0);
            spriteBatch.Draw(cannonBase.texture,
                cannonBase.position,
                null,
                Color.White,
                cannonBase.rotation,
                cannonBase.center,
                1.0f,
                SpriteEffects.None,
                0);

            spriteBatch.DrawString(spriteFont, "Score: " + score, new Vector2(20, 20), Color.Yellow);
            spriteBatch.Draw(healtBarOutline, healthBarOutlinePosition, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, healthBarColor);
        }

        private void EndGame()
        {
            currentGameState = GameState.HIGH_SCORES;
            highScoresScreen.EnterScoreMode(score);
            ResetGame();
        }

        private void ResetGame()
        {
            health = MAX_HEALTH;
            score = 0;
            foreach (GameObject enemy in enemies)
            {
                enemy.alive = false;
            }

            foreach (GameObject missile in missiles)
            {
                missile.alive = false;
            }

            foreach (GameObject ball in cannonBalls)
            {
                ball.alive = false;
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (currentGameState)
            {
                case GameState.GAME:
                    {
                        DrawGame(spriteBatch);
                        break;
                    }
                case GameState.MENU:
                    {
                        menuScreen.Draw(spriteBatch, viewportRectangle);
                        break;
                    }
                case GameState.HIGH_SCORES:
                    {
                        highScoresScreen.Draw(spriteBatch, viewportRectangle);
                        break;
                    }
                case GameState.CREDITS:
                    {
                        creditsScreen.Draw(spriteBatch, viewportRectangle);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
