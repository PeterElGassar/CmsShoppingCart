namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideWishListTbl : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.WishLists");
            AddPrimaryKey("dbo.WishLists", new[] { "productId", "UserId" });
            DropColumn("dbo.WishLists", "id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WishLists", "id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.WishLists");
            AddPrimaryKey("dbo.WishLists", "id");
        }
    }
}
