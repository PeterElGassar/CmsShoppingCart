namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brandValidation1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Brands", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Brands", "ImageName", c => c.String(nullable: false));
        }
    }
}
