namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropSliderGalleryTbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SliderGalleries", "Title", c => c.String());
            AddColumn("dbo.SliderGalleries", "UrlLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SliderGalleries", "UrlLink");
            DropColumn("dbo.SliderGalleries", "Title");
        }
    }
}
