namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenemeSlugPropAddQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Slug", c => c.String());
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Categories", "Sulg");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Sulg", c => c.String());
            DropColumn("dbo.Products", "Quantity");
            DropColumn("dbo.Categories", "Slug");
        }
    }
}
