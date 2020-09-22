namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WishListTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        productId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.productId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishLists", "UserId", "dbo.Users");
            DropForeignKey("dbo.WishLists", "productId", "dbo.Products");
            DropIndex("dbo.WishLists", new[] { "productId" });
            DropIndex("dbo.WishLists", new[] { "UserId" });
            DropTable("dbo.WishLists");
        }
    }
}
