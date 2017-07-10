﻿// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.PurchaseAnimalsMenu
// Assembly: Stardew Valley, Version=1.0.6118.35538, Culture=neutral, PublicKeyToken=null
// MVID: 91D9A392-4109-49BC-9B2D-A9A061D06895
// Assembly location: C:\WORK\GAME_DEV\StarDew\SDV-Mods\SimpleSower\bin\Debug\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace StardewValley.Menus
{
    public class MailOrderPigMenu : IClickableMenu
    {
        public static int menuHeight = Game1.tileSize * 5;
        public static int menuWidth = Game1.tileSize * 7;
        private List<ClickableTextureComponent> animalsToPurchase = new List<ClickableTextureComponent>();
        private ClickableTextureComponent okButton;
        private ClickableTextureComponent doneNamingButton;
        private ClickableTextureComponent randomButton;
        private ClickableTextureComponent hovered;
        private ClickableTextureComponent backButton;
        //private bool onFarm;
        private bool namingAnimal;
        private bool freeze;
        private FarmAnimal animalBeingPurchased;
        private TextBox textBox;
        private TextBoxEvent e;
        private Building newAnimalHome;
        private int priceOfAnimal;

        private bool condition = false;

        public MailOrderPigMenu(List<StardewValley.Object> stock)
          : base(Game1.viewport.Width / 2 - MailOrderPigMenu.menuWidth / 2 - IClickableMenu.borderWidth * 2, Game1.viewport.Height / 2 - MailOrderPigMenu.menuHeight - IClickableMenu.borderWidth * 2, MailOrderPigMenu.menuWidth + IClickableMenu.borderWidth * 2, MailOrderPigMenu.menuHeight + IClickableMenu.borderWidth, false)
        {
            this.height += Game1.tileSize;
            for (int index = 0; index < stock.Count; ++index)
            {
                List<ClickableTextureComponent> animalsToPurchase = this.animalsToPurchase;
                ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(string.Concat((object)stock[index].salePrice()), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + index % 3 * Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + index / 3 * (Game1.tileSize + Game1.tileSize / 3), Game1.tileSize * 2, Game1.tileSize), (string)null, stock[index].Name, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(index % 3 * 16 * 2, 448 + index / 3 * 16, 32, 16), 4f, stock[index].type == null);
                textureComponent1.item = (Item)stock[index];
                ClickableTextureComponent textureComponent2 = textureComponent1;
                animalsToPurchase.Add(textureComponent2);
            }
            this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
            this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
            MailOrderPigMenu.menuHeight = Game1.tileSize * 5;
            MailOrderPigMenu.menuWidth = Game1.tileSize * 7;
            this.textBox = new TextBox((Texture2D)null, (Texture2D)null, Game1.dialogueFont, Game1.textColor);
            this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
            this.textBox.Y = Game1.viewport.Height / 2;
            this.textBox.Width = Game1.tileSize * 4;
            this.textBox.Height = Game1.tileSize * 3;
            this.e = new TextBoxEvent(this.textBoxEnter);
            this.randomButton = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
            this.doneNamingButton = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - 10, this.yPositionOnScreen + 10, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);

        }

        public void textBoxEnter(TextBox sender)
        {
            if (!this.namingAnimal)
                return;
            if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is MailOrderPigMenu))
            {
                this.textBox.OnEnterPressed -= this.e;
            }
            else
            {
                if (sender.Text.Length < 1)
                    return;
                if (Utility.areThereAnyOtherAnimalsWithThisName(sender.Text))
                {
                    Game1.showRedMessage("Name Unavailable");
                }
                else
                {
                    this.newAnimalHome = ((AnimalHouse)Game1.player.currentLocation).getBuilding();
                    this.textBox.OnEnterPressed -= this.e;
                    this.animalBeingPurchased.name = sender.Text;
                    //StardewLib.Log.ERROR("Named Animal: " + sender.Text);
                    this.animalBeingPurchased.home = ((AnimalHouse)Game1.player.currentLocation).getBuilding();
                    this.animalBeingPurchased.homeLocation = new Vector2((float)this.newAnimalHome.tileX, (float)this.newAnimalHome.tileY);
                    this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
                    (this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
                    (this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
                    this.newAnimalHome = (Building)null;
                    Game1.player.money -= this.priceOfAnimal;
                    this.namingAnimal = false;

                    //Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnAfterPurchasingAnimal), 0.02f);
                    Game1.globalFadeToClear((Game1.afterFadeFunction)null, 0.02f);
                    this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
                    Game1.displayHUD = true;
                    Game1.displayFarmer = true;
                    this.freeze = false;
                    this.textBox.OnEnterPressed -= this.e;
                    this.textBox.Selected = false;
                    Game1.viewportFreeze = false;
                    Game1.globalFadeToClear(new Game1.afterFadeFunction(this.marnieAnimalPurchaseMessage), 0.02f);
                }
            }
        }

        public void setUpForReturnToShopMenu()
        {
            Game1.globalFadeToClear((Game1.afterFadeFunction)null, 0.02f);
            this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
            this.okButton.bounds.Y = this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth;
            Game1.displayHUD = true;
            Game1.viewportFreeze = false;
            this.namingAnimal = false;
            this.textBox.OnEnterPressed -= this.e;
            this.textBox.Selected = false;
        }

        public void setUpForReturnAfterPurchasingAnimal()
        {
            Game1.globalFadeToClear((Game1.afterFadeFunction)null, 0.02f);
            this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
            Game1.displayHUD = true;
            Game1.displayFarmer = true;
            this.freeze = false;
            this.textBox.OnEnterPressed -= this.e;
            this.textBox.Selected = false;
            Game1.viewportFreeze = false;
            Game1.globalFadeToClear(new Game1.afterFadeFunction(this.marnieAnimalPurchaseMessage), 0.02f);
        }

        public void marnieAnimalPurchaseMessage()
        {
            this.exitThisMenu(true);
            Game1.player.forceCanMove();
            this.freeze = false;

            Game1.activeClickableMenu = PelicanFiber.PelicanFiber.getMailOrderPigMenu();
        }

        private void backButtonPressed()
        {
            if (this.readyToClose())
            {
                this.exitThisMenu();
                PelicanFiber.PelicanFiber.showTheMenu();
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (Game1.globalFade || this.freeze)
                return;

            if (this.backButton.containsPoint(x, y))
            {
                this.backButton.scale = this.backButton.baseScale;
                backButtonPressed();
            }

            if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
            {
                if (namingAnimal)
                {
                    Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
                    Game1.playSound("smallSelect");
                }
                else
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
            }

            if (namingAnimal == true)
            {
                this.textBox.OnEnterPressed += this.e;
                Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber)this.textBox;
                //this.textBox.Text = this.animalBeingPurchased.name;
                this.textBox.Selected = true;

                if (this.doneNamingButton.containsPoint(x, y))
                {
                    this.animalBeingPurchased.name = this.textBox.Text;
                    this.textBoxEnter(this.textBox);
                    Game1.playSound("smallSelect");
                }
                else
                {
                    if (this.randomButton.containsPoint(x, y))
                    {
                        this.animalBeingPurchased.name = Dialogue.randomName();
                        this.textBox.Text = this.animalBeingPurchased.name;
                        this.randomButton.scale = this.randomButton.baseScale;
                        Game1.playSound("drumkit6");
                    }                 
                }
            }    

            foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
            {
                if (textureComponent.containsPoint(x, y) && (textureComponent.item as StardewValley.Object).type == null)
                {
                    int int32 = Convert.ToInt32(textureComponent.name);
                    if (Game1.player.money >= int32)
                    {
                        //Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForAnimalPlacement), 0.02f);
                        //Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForAnimalPlacement), 0.02f);
                        Game1.playSound("smallSelect");
                        //this.onFarm = true;
                        this.animalBeingPurchased = new FarmAnimal(textureComponent.hoverText, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
                        this.priceOfAnimal = int32;

                        //this.newAnimalHome = ((AnimalHouse)Game1.player.currentLocation).getBuilding();
                        //this.animalBeingPurchased.name = "John" + new Random().NextDouble();
                        //this.animalBeingPurchased.home = this.newAnimalHome;
                        //this.animalBeingPurchased.homeLocation = new Vector2((float)this.newAnimalHome.tileX, (float)this.newAnimalHome.tileY);
                        //this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
                        //(this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
                        //(this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
                        //this.newAnimalHome = (Building)null;
                        //this.namingAnimal = false;
                        //Game1.player.money -= this.priceOfAnimal;

                        //Game1.exitActiveMenu();
                        this.namingAnimal = true;
                    }
                    else
                        Game1.addHUDMessage(new HUDMessage("Not Enough Money", Color.Red, 3500f));
                }
            }

        }

        public override void receiveKeyPress(Keys key)
        {
            if (Game1.globalFade || this.freeze)
                return;

            if (!Game1.options.doesInputListContain(Game1.options.menuButton, key) || Game1.globalFade || !this.readyToClose())
                return;

            if (this.namingAnimal)
                return;

            Game1.player.forceCanMove();
            Game1.exitActiveMenu();
            Game1.playSound("bigDeSelect");
        }

        public override void update(GameTime time)
        {
            base.update(time);
            if (this.namingAnimal)
                return;

            //int num1 = Game1.getOldMouseX() + Game1.viewport.X;
            //int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
            //if (num1 - Game1.viewport.X < Game1.tileSize)
            //    Game1.panScreen(-8, 0);
            //else if (num1 - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
            //    Game1.panScreen(8, 0);
            //if (num2 - Game1.viewport.Y < Game1.tileSize)
            //    Game1.panScreen(0, -8);
            //else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
            //    Game1.panScreen(0, 8);
            //foreach (Keys pressedKey in Game1.oldKBState.GetPressedKeys())
            //    this.receiveKeyPress(pressedKey);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void performHoverAction(int x, int y)
        {
            this.hovered = (ClickableTextureComponent)null;
            if (Game1.globalFade || this.freeze)
                return;
            if (this.okButton != null)
            {
                if (this.okButton.containsPoint(x, y))
                    this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
                else
                    this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
            }

            if (namingAnimal)
            {
                if (this.doneNamingButton != null)
                {
                    if (this.doneNamingButton.containsPoint(x, y))
                        this.doneNamingButton.scale = Math.Min(1.1f, this.doneNamingButton.scale + 0.05f);
                    else
                        this.doneNamingButton.scale = Math.Max(1f, this.doneNamingButton.scale - 0.05f);
                }
                this.randomButton.tryHover(x, y, 0.5f);
            }
            else
            {
                foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
                {
                    if (textureComponent.containsPoint(x, y))
                    {
                        textureComponent.scale = Math.Min(textureComponent.scale + 0.05f, 4.1f);
                        this.hovered = textureComponent;
                    }
                    else
                        textureComponent.scale = Math.Max(4f, textureComponent.scale - 0.025f);
                }
            }
        }

        public static string getAnimalDescription(string name)
        {
            switch (name)
            {
                case "Chicken":
                    return "Well cared-for adult chickens lay eggs every day." + Environment.NewLine + "Lives in the coop.";
                case "Duck":
                    return "Happy adults lay duck eggs every other day." + Environment.NewLine + "Lives in the coop.";
                case "Rabbit":
                    return "These are wooly rabbits! They shed precious wool every few days." + Environment.NewLine + "Lives in the coop.";
                case "Dairy Cow":
                    return "Adults can be milked daily. A milk pail is required to harvest the milk." + Environment.NewLine + "Lives in the barn.";
                case "Pig":
                    return "These pigs are trained to find truffles!" + Environment.NewLine + "Lives in the barn.";
                case "Goat":
                    return "Happy adults provide goat milk every other day. A milk pail is required to harvest the milk." + Environment.NewLine + "Lives in the barn.";
                case "Sheep":
                    return "Adults can be shorn for wool. Sheep who form a close bond with their owners can grow wool faster. A pair of shears is required to harvest the wool." + Environment.NewLine + "Lives in the barn.";
                default:
                    return "";
            }
        }

        public override void draw(SpriteBatch b)
        {
            if (!Game1.dialogueUp && !Game1.globalFade)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                SpriteText.drawStringWithScrollBackground(b, "Livestock:", this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen, "", 1f, -1);
                Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string)null, false);
                Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
                foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
                    textureComponent.draw(b, (textureComponent.item as StardewValley.Object).type != null ? Color.Black * 0.4f : Color.White, 0.87f);

                this.backButton.draw(b);
            }
            if (!Game1.globalFade && this.okButton != null)
                this.okButton.draw(b);

            if (this.namingAnimal)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 4, Game1.viewport.Height / 2 - Game1.tileSize * 3 - Game1.tileSize / 2, Game1.tileSize * 8, Game1.tileSize * 3, false, true, (string)null, false);
                Utility.drawTextWithShadow(b, "Name your new animal: ", Game1.dialogueFont, new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 4 + Game1.tileSize / 2 + 8), (float)(Game1.viewport.Height / 2 - Game1.tileSize * 2 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
                this.textBox.Draw(b);
                this.doneNamingButton.draw(b);
                this.randomButton.draw(b);
            }

            if (this.hovered != null)
            {
                if ((this.hovered.item as StardewValley.Object).type != null)
                {
                    IClickableMenu.drawHoverText(b, Game1.parseText((this.hovered.item as StardewValley.Object).type, Game1.dialogueFont, Game1.tileSize * 5), Game1.dialogueFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);
                }
                else
                {
                    SpriteText.drawStringWithScrollBackground(b, this.hovered.hoverText, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize, this.yPositionOnScreen + this.height + -Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "Truffle Pig", 1f, -1);
                    SpriteText.drawStringWithScrollBackground(b, "$" + this.hovered.name + "g", this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, this.yPositionOnScreen + this.height + Game1.tileSize + IClickableMenu.spaceToClearTopBorder / 2 + 8, "$99999g", Game1.player.Money >= Convert.ToInt32(this.hovered.name) ? 1f : 0.5f, -1);
                    IClickableMenu.drawHoverText(b, Game1.parseText(MailOrderPigMenu.getAnimalDescription(this.hovered.hoverText), Game1.smallFont, Game1.tileSize * 5), Game1.smallFont, 0, 0, -1, this.hovered.hoverText, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);
                }
            }
            this.drawMouse(b);
        }
    }
}