namespace Writers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.GenreID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        FullName = c.String(nullable: false, maxLength: 50),
                        BirthDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        BirthPlace = c.String(maxLength: 50),
                        DieDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DiePlace = c.String(maxLength: 50),
                        Century = c.String(maxLength: 15),
                        Country = c.String(maxLength: 31),
                        Occupation = c.String(maxLength: 100),
                        Education = c.String(maxLength: 100),
                        Biography = c.String(),
                    })
                .PrimaryKey(t => t.FullName);
            
            CreateTable(
                "dbo.PersonsImages",
                c => new
                    {
                        PersonFullName = c.String(nullable: false, maxLength: 50),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.PersonFullName)
                .ForeignKey("dbo.People", t => t.PersonFullName)
                .Index(t => t.PersonFullName);
            
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        WorkID = c.Int(nullable: false, identity: true),
                        AuthorFullName = c.String(),
                        Title = c.String(),
                        ReleaseYear = c.Int(nullable: false),
                        ReleasePlace = c.String(),
                        Person_FullName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.WorkID)
                .ForeignKey("dbo.People", t => t.Person_FullName)
                .Index(t => t.Person_FullName);
            
            CreateTable(
                "dbo.PersonGenres",
                c => new
                    {
                        Person_FullName = c.String(nullable: false, maxLength: 50),
                        Genre_GenreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_FullName, t.Genre_GenreID })
                .ForeignKey("dbo.People", t => t.Person_FullName, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .Index(t => t.Person_FullName)
                .Index(t => t.Genre_GenreID);
            
            CreateTable(
                "dbo.WorkGenres",
                c => new
                    {
                        Work_WorkID = c.Int(nullable: false),
                        Genre_GenreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Work_WorkID, t.Genre_GenreID })
                .ForeignKey("dbo.Works", t => t.Work_WorkID, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .Index(t => t.Work_WorkID)
                .Index(t => t.Genre_GenreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Works", "Person_FullName", "dbo.People");
            DropForeignKey("dbo.WorkGenres", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.WorkGenres", "Work_WorkID", "dbo.Works");
            DropForeignKey("dbo.PersonsImages", "PersonFullName", "dbo.People");
            DropForeignKey("dbo.PersonGenres", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.PersonGenres", "Person_FullName", "dbo.People");
            DropIndex("dbo.WorkGenres", new[] { "Genre_GenreID" });
            DropIndex("dbo.WorkGenres", new[] { "Work_WorkID" });
            DropIndex("dbo.PersonGenres", new[] { "Genre_GenreID" });
            DropIndex("dbo.PersonGenres", new[] { "Person_FullName" });
            DropIndex("dbo.Works", new[] { "Person_FullName" });
            DropIndex("dbo.PersonsImages", new[] { "PersonFullName" });
            DropTable("dbo.WorkGenres");
            DropTable("dbo.PersonGenres");
            DropTable("dbo.Works");
            DropTable("dbo.PersonsImages");
            DropTable("dbo.People");
            DropTable("dbo.Genres");
        }
    }
}
