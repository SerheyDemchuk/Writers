namespace Writers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedWorkModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkGenres", "Work_AuthorFullName", "dbo.Works");
            DropIndex("dbo.WorkGenres", new[] { "Work_AuthorFullName" });
            RenameColumn(table: "dbo.WorkGenres", name: "Work_AuthorFullName", newName: "Work_WorkID");
            DropPrimaryKey("dbo.Works");
            DropPrimaryKey("dbo.WorkGenres");
            AddColumn("dbo.Works", "WorkID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Works", "AuthorFullName", c => c.String());
            AlterColumn("dbo.WorkGenres", "Work_WorkID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Works", "WorkID");
            AddPrimaryKey("dbo.WorkGenres", new[] { "Work_WorkID", "Genre_GenreID" });
            CreateIndex("dbo.WorkGenres", "Work_WorkID");
            AddForeignKey("dbo.WorkGenres", "Work_WorkID", "dbo.Works", "WorkID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkGenres", "Work_WorkID", "dbo.Works");
            DropIndex("dbo.WorkGenres", new[] { "Work_WorkID" });
            DropPrimaryKey("dbo.WorkGenres");
            DropPrimaryKey("dbo.Works");
            AlterColumn("dbo.WorkGenres", "Work_WorkID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Works", "AuthorFullName", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Works", "WorkID");
            AddPrimaryKey("dbo.WorkGenres", new[] { "Work_AuthorFullName", "Genre_GenreID" });
            AddPrimaryKey("dbo.Works", "AuthorFullName");
            RenameColumn(table: "dbo.WorkGenres", name: "Work_WorkID", newName: "Work_AuthorFullName");
            CreateIndex("dbo.WorkGenres", "Work_AuthorFullName");
            AddForeignKey("dbo.WorkGenres", "Work_AuthorFullName", "dbo.Works", "AuthorFullName", cascadeDelete: true);
        }
    }
}
