namespace Salon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeerMigration : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AspNetUsers", "studentNumber", c => c.String(maxLength: 20));
            //AddColumn("dbo.AspNetUsers", "ChangedPassword", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.AspNetUsers", "ChangedPassword");
            //DropColumn("dbo.AspNetUsers", "studentNumber");
        }
    }
}
