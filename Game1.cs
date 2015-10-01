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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace catch_the_babies
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D mMenu;
        Texture2D mPlayer;
        Texture2D mBaby;
        Texture2D mBackground;
        Texture2D mDeadBaby;
        Texture2D mPauseMenu;
        Texture2D mGameOver;
        Texture2D mCreditsMenu;
        SpriteFont sf, lhi;
        Vector2 playerPosition = new Vector2(250, 425);
        Vector2 babyPos = new Vector2 (175, 250);
        Vector2 mPos = new Vector2(0, 0);
        Vector2 goScorePos = new Vector2(515, 120);
        Vector2 deadBaby1Pos;
        Vector2 deadBaby2Pos;
        Vector2 deadBaby3Pos;
        Vector2 playGame;
        Vector2 Credits;
        Vector2 ScorePos = new Vector2(200, 575);//600);        
        Random rndX = new Random();
        SoundEffect soundEngine, giggle, scream, splatter;
        SoundEffectInstance soundEngineInstance, giggleInstance, ScreamInstance, splatterInstance;
        int Score, deadBabies, Level, Speed, ix, inum, menuIndex, optionsIndex, creditsIndex, goIndex;
        bool isMenu, isAlive, isFalling, isGameOver, isPaused, isCredits;
        
        public Game1()
        {
                      
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 655;//648;
            graphics.PreferredBackBufferWidth = 854;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
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
            isMenu = true;
            isPaused = false;
            isGameOver = false;
            isFalling = true;
            Level = 1;
            Score = 0;
            Speed = 3;
            playGame = new Vector2(285, 150);
            Credits = new Vector2(285, 210);
            menuIndex = 1;
            creditsIndex = 1;
            goIndex = 1;
            optionsIndex = 1;
            this.Components.Add(new GamerServicesComponent(this));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //Now heres where you are loading the images correct? images and fonts that will be used yes. also sounds when i get to that
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sf = this.Content.Load<SpriteFont>("menufont");
            lhi = this.Content.Load<SpriteFont>("LHI");
            mMenu = this.Content.Load<Texture2D>("menu");
            mBackground = this.Content.Load<Texture2D>("town");
            mPlayer = this.Content.Load<Texture2D>("glove");
            mDeadBaby = this.Content.Load<Texture2D>("deadbaby");
            mBaby = this.Content.Load<Texture2D>("baby");
            mPauseMenu = this.Content.Load<Texture2D>("pause");
            mCreditsMenu = this.Content.Load<Texture2D>("credits");
            mGameOver = this.Content.Load<Texture2D>("gameover");
            soundEngine = this.Content.Load<SoundEffect>("audioloop");
            giggle = this.Content.Load<SoundEffect>("babygigl");
            scream = this.Content.Load<SoundEffect>("babyScream");
            splatter = this.Content.Load<SoundEffect>("splat");
            soundEngineInstance = soundEngine.CreateInstance();
            giggleInstance = giggle.CreateInstance();
            ScreamInstance = scream.CreateInstance();
            splatterInstance = splatter.CreateInstance();
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
              //  this.Exit();
            
            if (isMenu)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    if (menuIndex < 2)
                    {
                        menuIndex += 1;
                                             
                    }
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    if (menuIndex > 1)
                    {
                        menuIndex -= 1;
                    }
                }
                if (menuIndex == 1 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || menuIndex == 1 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                        isMenu = false;
                        isAlive = true;
                        isGameOver = false;
                        isPaused = false;
                        isFalling = true;
                        Speed = 3;
                }
                else if (menuIndex == 2 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)|| menuIndex == 2 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                    isCredits = true;
                    isMenu = false;
                    isAlive = false;
                    isPaused = false;
                    isFalling = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
            }
            else if (isCredits)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    if (creditsIndex < 2)
                    {
                        creditsIndex += 1;

                    }
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    if (creditsIndex > 1)
                    {
                        creditsIndex -= 1;
                    }
                }
                if (creditsIndex == 2 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) || creditsIndex == 2 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                {
                    isCredits = false;
                    isMenu = true;
                    isPaused = false;
                }
                if (creditsIndex == 1 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) || creditsIndex == 1 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                {

                        isMenu = false;
                        isCredits = false;
                        isAlive = true;
                        isGameOver = false;
                        isPaused = false;
                        isFalling = true;
                        Speed = 3;
                }
                
            }
            else if (isPaused)
            {
                if (optionsIndex == 1 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || optionsIndex ==1 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                    isPaused = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                {
                    this.Exit();
                }
                else if (optionsIndex == 2 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || optionsIndex == 2 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                    isAlive = false;
                    isGameOver = false;
                    isPaused = false;
                    isMenu = true;
                    isFalling = false;
                    Score = 0;
                    Level = 1;
                    deadBabies = 0;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    if (optionsIndex < 2)
                    {
                        optionsIndex += 1;

                    }
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    if (optionsIndex > 1)
                    {
                        optionsIndex -= 1;
                    }
                }

            }
            else if (isGameOver)
            {
               
                if (goIndex == 1 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || goIndex == 1 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                        isGameOver = false;
                        isMenu = false;
                        Score = 0;
                        Level = 1;
                        deadBabies = 0;
                        Speed = 3;
                        isAlive = true;
                        isFalling = true;
                    
                }
                else if (goIndex == 2 && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || goIndex == 2 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                    this.Exit();
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown)|| GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    if (goIndex < 2)
                    {
                        goIndex += 1;

                    }
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp)|| GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    if (goIndex > 1)
                    {
                        goIndex -= 1;
                    }
                }

            }
            else
            {
                if (isAlive)
                {
                    //coding for falling babies
                    soundEngineInstance.Volume = 1.00f;
                    soundEngineInstance.Play();
                    if (isFalling)
                    {
                        babyPos.Y += Speed;
                        ScreamInstance.Play(); 
                        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                        if (babyPos.X - playerPosition.X <= 100 && babyPos.X - playerPosition.X >= 0 && babyPos.Y >= 425)
                        {
                            Score += 100;
                            ScreamInstance.Stop();
                            giggleInstance.Volume = 1.0f;
                            giggleInstance.Play();
                            if (inum < 10)
                            {
                                inum += 1;
                                giggleInstance.Stop();
                            }
                            else if (inum == 10)
                            {
                                inum = 0;
                                Level += 1;
                                Speed += 1;
                                giggleInstance.Stop();

                            }
                        }
                        else if (babyPos.Y >= 425)
                        {
                            
                            deadBabies += 1;
                            ScreamInstance.Stop();
                            splatterInstance.Play();
                            Score -= 50;
                            if (Score < 0)
                            {
                                Score = 0;
                            }
                            if (deadBabies == 1)
                            {
                                deadBaby1Pos = new Vector2(ix, 460);
                                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                            }
                            if (deadBabies == 2)
                            {
                                deadBaby2Pos = new Vector2(ix, 460);
                                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                            }
                            if (deadBabies == 3)
                            {
                                deadBaby3Pos = new Vector2(ix, 460);
                                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                            }
                            if (deadBabies == 4)
                            {
                                isGameOver = true;
                                isPaused = false;
                                isAlive = false;
                                isFalling = false;
                                ScreamInstance.Stop();
                                soundEngineInstance.Stop();
                                giggleInstance.Play();
                                GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                            }
                             
                        }
                        
                        if (babyPos.Y >= 425)
                        {
                            ix = rndX.Next(15, 690);
                            babyPos.X = ix;
                            babyPos.Y = 0;
                        }

                        
                    }
                    

                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
                    {
                        if (playerPosition.X >= 10)
                        {
                            playerPosition.X -= 6;
                        }
                    }
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
                    {
                        if (playerPosition.X <= 638)
                        {
                            playerPosition.X += 6;
                        }
                    }
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                    {
                        if (isPaused != true)
                        {
                            isPaused = true;
                        }

                    }
                }
                else if (isAlive != true)
                {
                    isGameOver = true;
                    isMenu = false;
                    isPaused = false;
                }
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
                 
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            if (isMenu)
            {
                spriteBatch.Draw(mMenu, mPos, Color.White);
                if (menuIndex == 1)
                {
                    spriteBatch.DrawString(lhi, "Play Game", playGame, Color.White);
                    spriteBatch.DrawString(lhi, "Credits", Credits, Color.YellowGreen);
                }
                else
                {
                    spriteBatch.DrawString(lhi, "Play Game", playGame, Color.YellowGreen);
                    spriteBatch.DrawString(lhi, "Credits", Credits, Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(mBackground, mPos, Color.White);

                if (isAlive)
                {
                    
                    if (isPaused != true)
                    {                   
                        if (deadBabies == 1)
                        {
                            spriteBatch.Draw(mDeadBaby, deadBaby1Pos, Color.White);
                        }
                        if (deadBabies == 2)
                        {
                            spriteBatch.Draw(mDeadBaby, deadBaby1Pos, Color.White);
                            spriteBatch.Draw(mDeadBaby, deadBaby2Pos, Color.White);
                        }
                        if (deadBabies == 3)
                        {
                            spriteBatch.Draw(mDeadBaby, deadBaby1Pos, Color.White);
                            spriteBatch.Draw(mDeadBaby, deadBaby2Pos, Color.White);
                            spriteBatch.Draw(mDeadBaby, deadBaby3Pos, Color.White);
                        }

                        spriteBatch.Draw(mPlayer, playerPosition, Color.White);

                        if (isFalling)
                        {
                            spriteBatch.Draw(mBaby, babyPos, Color.White);

                        }

                        spriteBatch.DrawString(sf, "Level: " + Level + " - Your Score: " + Score + " - Deaths: " + deadBabies, ScorePos, Color.YellowGreen);
                    }
                   

                }
                if (isGameOver)
                {
                    Vector2 playagain = new Vector2(275, 250);
                    Vector2 exit = new Vector2(275, 310);
                    string x = Score.ToString();
                    spriteBatch.Draw(mGameOver, mPos, Color.White);
                    spriteBatch.DrawString(lhi, x, goScorePos, Color.Yellow);
                    if (goIndex == 1)
                    {
                        spriteBatch.DrawString(lhi, "Play Again", playagain, Color.White);
                        spriteBatch.DrawString(lhi, "Exit", exit, Color.YellowGreen);
                    }
                    else
                    {
                        spriteBatch.DrawString(lhi, "Play Again", playagain, Color.YellowGreen);
                        spriteBatch.DrawString(lhi, "Exit", exit, Color.White);
                    }
                    
                }
                if (isCredits)
                {
                    Vector2 creditsDevelopedby = new Vector2(200, 125);
                    Vector2 withHelpFrom = new Vector2(200, 165);
                    Vector2 consols = new Vector2(200, 205);
                    Vector2 visit = new Vector2(200, 245);
                    Vector2 cheesecakestudios = new Vector2(250, 285);
                    Vector2 miker525 = new Vector2(250, 325);
                    Vector2 orFollow = new Vector2(200, 365);
                    Vector2 twitter = new Vector2(250, 405);
                    Vector2 gameplay = new Vector2(250, 485);
                    Vector2 Return = new Vector2(250, 545);
                    spriteBatch.Draw(mCreditsMenu, mPos, Color.White);
                    spriteBatch.DrawString (sf, "Developed by Mike Rosenberg", creditsDevelopedby, Color.YellowGreen);
                    spriteBatch.DrawString(sf, "With Help From Josh & Moskie", withHelpFrom, Color.YellowGreen);
                    spriteBatch.DrawString(sf, "Playable On PC and Xbox 360", consols, Color.YellowGreen);
                    spriteBatch.DrawString(sf, "For More Games and Apps Please Visit:", visit, Color.YellowGreen);
                    spriteBatch.DrawString(sf, "http://www.cheesecakestudios.org", cheesecakestudios, Color.SteelBlue);
                    spriteBatch.DrawString(sf, "http://www.miker525.info", miker525, Color.SteelBlue);
                    spriteBatch.DrawString(sf, "Or Follow Me On Twitter:", orFollow, Color.YellowGreen);
                    spriteBatch.DrawString(sf, "http://www.twitter.com/miker525", twitter, Color.SteelBlue);
                    if (creditsIndex == 1)
                    {
                        spriteBatch.DrawString(lhi, "Play Game", gameplay, Color.White);
                        spriteBatch.DrawString(lhi, "Return To Menu", Return, Color.YellowGreen);
                    }
                    else
                    {
                        spriteBatch.DrawString(lhi, "Play Game", gameplay, Color.YellowGreen);
                        spriteBatch.DrawString(lhi, "Return To Menu", Return, Color.White);
                    }
                }
                if (isPaused)
                {
                    spriteBatch.Draw(mPauseMenu, mPos, Color.White);
                    if (optionsIndex == 1)
                    {
                        spriteBatch.DrawString(lhi, "Resume Game", playGame, Color.White);
                        spriteBatch.DrawString(lhi, "Restart Game", Credits, Color.YellowGreen);

                    }
                    else if (optionsIndex == 2)
                    {
                        spriteBatch.DrawString(lhi, "Resume Game", playGame, Color.YellowGreen);
                        spriteBatch.DrawString(lhi, "Restart Game", Credits, Color.White);

                    }
                    
                }
            }
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
