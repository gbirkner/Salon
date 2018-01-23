namespace Salon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "firstName", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "lastName", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Class", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "entryDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "resignationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "resignationDate");
            DropColumn("dbo.AspNetUsers", "entryDate");
            DropColumn("dbo.AspNetUsers", "Class");
            DropColumn("dbo.AspNetUsers", "lastName");
            DropColumn("dbo.AspNetUsers", "firstName");
        }
    }
}
