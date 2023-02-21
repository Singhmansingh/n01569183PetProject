namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "TeamHasImg", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Teams", "TeamImgExt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "TeamImgExt");
            DropColumn("dbo.Teams", "TeamHasImg");
        }
    }
}
