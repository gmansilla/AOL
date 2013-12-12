using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    class Thwomp : Boss
    {
        Vector2 position;
        const float normalSpeed = 7f;
        const float attackSpeed = 14f;
       
        public Thwomp(Texture2D newImage, AnimationInfo[] newAnimationInfo) :
                 this(new Sprite(newImage, Vector2.Zero, newAnimationInfo, 1f))
        { }

        Thwomp(Sprite newSprite)
        {
            hp = MaxHP;
            type = Global.BabyMetroid;

            sprite = newSprite;
            position = sprite.Position;
            fightAreaTrigger = Global.InvalidBoundingBox;

            movement = new Dictionary<EnemyStatus, float>();
            movement.Add(EnemyStatus.Moving, normalSpeed);
            movement.Add(EnemyStatus.Stunned, normalSpeed);
            movement.Add(EnemyStatus.Attacking, attackSpeed);
            movement.Add(EnemyStatus.Hurt, normalSpeed);

            /*
            spinSpeed = new Dictionary<EnemyStatus, float>();
            spinSpeed.Add(EnemyStatus.Moving, normalSpinSpeed);
            spinSpeed.Add(EnemyStatus.Stunned, normalSpinSpeed);
            spinSpeed.Add(EnemyStatus.Attacking, attackSpinSpeed);
            spinSpeed.Add(EnemyStatus.Hurt, normalSpinSpeed);
            status = EnemyStatus.Moving;
            */
            //moveCount = 1;

            currentAnimation = Global.Moving;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, BoundingBox playerBounds, int screenWidth, bool inBossFight)
        {

        }

        public override void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            sprite.SetPosition(newPosition);
        }

        public override void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            sprite.Draw(spriteBatch, transparency);
        }
    }
}
