namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConstraintToSlider : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SliderGalleries", "ImageName", c => c.String(nullable: false));
            AlterColumn("dbo.SliderGalleries", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.SliderGalleries", "UrlLink", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SliderGalleries", "UrlLink", c => c.String());
            AlterColumn("dbo.SliderGalleries", "Title", c => c.String());
            AlterColumn("dbo.SliderGalleries", "ImageName", c => c.String());
        }
    }
}
