namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleExt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Roles", "RoleImgExt", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "RoleImgExt", c => c.Boolean(nullable: false));
        }
    }
}
