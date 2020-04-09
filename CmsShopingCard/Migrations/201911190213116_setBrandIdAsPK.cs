namespace CmsShopingCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class setBrandIdAsPK : DbMigration
    {
        public override void Up()
        {          

            AlterColumn("dbo.Brands", "BrandId", c => c.Int(nullable: false, identity: true));
        }

        public override void Down()
        {
        }
    }
}
