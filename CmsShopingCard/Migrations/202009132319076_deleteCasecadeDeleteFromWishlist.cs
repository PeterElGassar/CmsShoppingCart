namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteCasecadeDeleteFromWishlist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WishLists", "productId", "dbo.Products");
            AddForeignKey("dbo.WishLists", "productId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishLists", "productId", "dbo.Products");
            AddForeignKey("dbo.WishLists", "productId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
