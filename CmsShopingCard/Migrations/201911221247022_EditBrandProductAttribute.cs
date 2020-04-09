namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBrandProductAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "BrandId", c => c.Int(nullable: true));
            CreateIndex("dbo.Products", "BrandId");
            AddForeignKey("dbo.Products", "BrandId", "dbo.Brands", "BrandId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropColumn("dbo.Products", "BrandId");
        }
    }
}
