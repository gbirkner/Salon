namespace Salon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewsUserProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "studentNumber", c => c.String(maxLength: 20,nullable:true));
            AddColumn("dbo.AspNetUsers", "ChangedPassword", c => c.Boolean(defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "studentNumber");
            DropColumn("dbo.AspNetUsers", "ChangedPassword");
        }
    }
}
