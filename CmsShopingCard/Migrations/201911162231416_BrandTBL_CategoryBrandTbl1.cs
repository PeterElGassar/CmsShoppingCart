namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandTBL_CategoryBrandTbl1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brands", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Brands", "ImageName");
        }
    }
}
