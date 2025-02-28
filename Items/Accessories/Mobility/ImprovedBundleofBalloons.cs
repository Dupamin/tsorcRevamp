using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories.Mobility
{
    public class ImprovedBundleofBalloons : ModItem
    {
        public static float JumpSpeed = 120f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(JumpSpeed);
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.accessory = true;
            Item.value = PriceByRarity.Pink_5;
            Item.rare = ItemRarityID.Pink;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            RasterizerState OverflowHiddenRasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
            Texture2D texture = (Texture2D)Terraria.GameContent.TextureAssets.Item[Item.type];
            for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 3;
                spriteBatch.Draw(texture, position + offsetPositon, null, Color.GreenYellow, 0, origin, scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);

            return true;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += JumpSpeed / 100f;
            player.jumpBoost = true;
            player.GetJumpState(ExtraJump.CloudInABottle).Enable()/* tModPorter Suggestion: Call Enable() if setting this to true, otherwise call Disable(). */;
            player.GetJumpState(ExtraJump.BlizzardInABottle).Enable()/* tModPorter Suggestion: Call Enable() if setting this to true, otherwise call Disable(). */;
            player.GetJumpState(ExtraJump.SandstormInABottle).Enable()/* tModPorter Suggestion: Call Enable() if setting this to true, otherwise call Disable(). */;
        }

    }
}