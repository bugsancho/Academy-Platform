namespace AcademyPlatform.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateLectureVisit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LectureVisits", "ExternalLectureId", c => c.Int(nullable: false));
            Sql("UPDATE dbo.LectureVisits SET ExternalLectureId = LectureId");
            Sql(@"UPDATE
                    visit
                SET
                    visit.LectureId = lecture.Id
                FROM
                    dbo.LectureVisits visit
                INNER JOIN
                    dbo.Lectures lecture
                ON
                    lecture.ExternalId = visit.LectureId");
            CreateIndex("dbo.LectureVisits", "LectureId");
            AddForeignKey("dbo.LectureVisits", "LectureId", "dbo.Lectures", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.LectureVisits", "LectureId", "dbo.Lectures");
            DropIndex("dbo.LectureVisits", new[] { "LectureId" });
            DropColumn("dbo.LectureVisits", "ExternalLectureId");
        }
    }
}
