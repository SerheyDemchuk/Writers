namespace Writers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedImagesCollection : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Images");
            AddColumn("dbo.Images", "ImagesID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Images", "ImagesID");
            DropColumn("dbo.Images", "PersonFullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "PersonFullName", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Images");
            DropColumn("dbo.Images", "ImagesID");
            AddPrimaryKey("dbo.Images", "PersonFullName");
        }
    }
}
