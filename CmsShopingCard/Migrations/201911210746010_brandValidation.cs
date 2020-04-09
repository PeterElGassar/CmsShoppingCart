namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brandValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Brands", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Brands", "ImageName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Brands", "ImageName", c => c.String());
            AlterColumn("dbo.Brands", "Name", c => c.String());
        }
    }
}
