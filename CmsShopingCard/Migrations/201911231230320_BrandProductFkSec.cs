namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BrandProductFkSec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "BrandId", c => c.Int());
            CreateIndex("dbo.Products", "BrandId");
            AddForeignKey("dbo.Products", "BrandId", "dbo.Brands", "BrandId");
            // ADD THIS BY HAND
            //Sql(@"UPDATE dbo.MyEntity SET MyOtherEntityId = 1
            //  where MyOtherEntity IS NULL");

            Sql(@"UPDATE dbo.Products SET BrandId = 11 where BrandId IS NULL ");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropColumn("dbo.Products", "BrandId");
        }
    }
}
