namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetImgSliderImageNameOptinal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SliderGalleries", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SliderGalleries", "ImageName", c => c.String(nullable: false));
        }
    }
}
