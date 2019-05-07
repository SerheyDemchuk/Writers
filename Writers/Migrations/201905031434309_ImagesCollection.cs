namespace Writers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagesCollection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonsImages", "PersonFullName", "dbo.People");
            RenameTable(name: "dbo.PersonsImages", newName: "Images");
            DropForeignKey("dbo.WorkGenres", "Work_WorkID", "dbo.Works");
            DropIndex("dbo.Images", new[] { "PersonFullName" });
            DropIndex("dbo.WorkGenres", new[] { "Work_WorkID" });
            RenameColumn(table: "dbo.WorkGenres", name: "Work_WorkID", newName: "Work_AuthorFullName");
            DropPrimaryKey("dbo.Images");
            DropPrimaryKey("dbo.Works");
            DropPrimaryKey("dbo.WorkGenres");
            AddColumn("dbo.Images", "Person_FullName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Images", "PersonFullName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Works", "AuthorFullName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.WorkGenres", "Work_AuthorFullName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Images", "PersonFullName");
            AddPrimaryKey("dbo.Works", "AuthorFullName");
            AddPrimaryKey("dbo.WorkGenres", new[] { "Work_AuthorFullName", "Genre_GenreID" });
            CreateIndex("dbo.Images", "Person_FullName");
            CreateIndex("dbo.WorkGenres", "Work_AuthorFullName");
            AddForeignKey("dbo.Images", "Person_FullName", "dbo.People", "FullName");
            AddForeignKey("dbo.WorkGenres", "Work_AuthorFullName", "dbo.Works", "AuthorFullName", cascadeDelete: true);
            DropColumn("dbo.Works", "WorkID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Works", "WorkID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.WorkGenres", "Work_AuthorFullName", "dbo.Works");
            DropForeignKey("dbo.Images", "Person_FullName", "dbo.People");
            DropIndex("dbo.WorkGenres", new[] { "Work_AuthorFullName" });
            DropIndex("dbo.Images", new[] { "Person_FullName" });
            DropPrimaryKey("dbo.WorkGenres");
            DropPrimaryKey("dbo.Works");
            DropPrimaryKey("dbo.Images");
            AlterColumn("dbo.WorkGenres", "Work_AuthorFullName", c => c.Int(nullable: false));
            AlterColumn("dbo.Works", "AuthorFullName", c => c.String());
            AlterColumn("dbo.Images", "PersonFullName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Images", "Person_FullName");
            AddPrimaryKey("dbo.WorkGenres", new[] { "Work_WorkID", "Genre_GenreID" });
            AddPrimaryKey("dbo.Works", "WorkID");
            AddPrimaryKey("dbo.Images", "PersonFullName");
            RenameColumn(table: "dbo.WorkGenres", name: "Work_AuthorFullName", newName: "Work_WorkID");
            CreateIndex("dbo.WorkGenres", "Work_WorkID");
            CreateIndex("dbo.Images", "PersonFullName");
            AddForeignKey("dbo.WorkGenres", "Work_WorkID", "dbo.Works", "WorkID", cascadeDelete: true);
            AddForeignKey("dbo.PersonsImages", "PersonFullName", "dbo.People", "FullName");
            RenameTable(name: "dbo.Images", newName: "PersonsImages");
        }
    }
}
