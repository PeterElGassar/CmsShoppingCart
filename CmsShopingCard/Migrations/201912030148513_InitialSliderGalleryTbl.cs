namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSliderGalleryTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SliderGalleries",
                c => new
                    {
                        SliderId = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                    })
                .PrimaryKey(t => t.SliderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SliderGalleries");
        }
    }
}
