namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandsInCatVM : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryBrands", "BrandId", "dbo.Brands");
            DropPrimaryKey("dbo.Brands");
            DropColumn("dbo.Brands", "Id");

            AddColumn("dbo.Brands", "BrandId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Brands", "BrandId");
            AddForeignKey("dbo.CategoryBrands", "BrandId", "dbo.Brands", "BrandId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Brands", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.CategoryBrands", "BrandId", "dbo.Brands");
            DropPrimaryKey("dbo.Brands");
            DropColumn("dbo.Brands", "BrandId");
            AddPrimaryKey("dbo.Brands", "Id");
            AddForeignKey("dbo.CategoryBrands", "BrandId", "dbo.Brands", "Id", cascadeDelete: true);
        }
    }
}
