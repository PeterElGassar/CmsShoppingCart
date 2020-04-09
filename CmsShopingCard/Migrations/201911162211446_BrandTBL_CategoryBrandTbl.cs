namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandTBL_CategoryBrandTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryBrands",
                c => new
                    {
                        CategoryBrandId = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryBrandId)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: false)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .Index(t => t.BrandId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryBrands", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryBrands", "BrandId", "dbo.Brands");
            DropIndex("dbo.CategoryBrands", new[] { "CategoryId" });
            DropIndex("dbo.CategoryBrands", new[] { "BrandId" });
            DropTable("dbo.CategoryBrands");
            DropTable("dbo.Brands");
        }
    }
}
