using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AsqMe.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "aspnetroles",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(maxLength: 256, nullable: true),
                    normalizedname = table.Column<string>(maxLength: 256, nullable: true),
                    concurrencystamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aspnetusers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    username = table.Column<string>(maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(maxLength: 256, nullable: true),
                    email = table.Column<string>(maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(maxLength: 256, nullable: true),
                    emailconfirmed = table.Column<bool>(nullable: false),
                    passwordhash = table.Column<string>(nullable: true),
                    securitystamp = table.Column<string>(nullable: true),
                    concurrencystamp = table.Column<string>(nullable: true),
                    phonenumber = table.Column<string>(nullable: true),
                    phonenumberconfirmed = table.Column<bool>(nullable: false),
                    twofactorenabled = table.Column<bool>(nullable: false),
                    lockoutend = table.Column<DateTimeOffset>(nullable: true),
                    lockoutenabled = table.Column<bool>(nullable: false),
                    accessfailedcount = table.Column<int>(nullable: false),
                    createddate = table.Column<DateTime>(nullable: false),
                    updateddate = table.Column<DateTime>(nullable: false),
                    facebookname = table.Column<string>(nullable: true),
                    profilepicture = table.Column<string>(nullable: true),
                    profilepictureweb = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createddate = table.Column<DateTime>(nullable: false),
                    updateddate = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    imageoricon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createddate = table.Column<DateTime>(nullable: false),
                    updateddate = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aspnetroleclaims",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<string>(nullable: false),
                    claimtype = table.Column<string>(nullable: true),
                    claimvalue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roleclaims", x => x.id);
                    table.ForeignKey(
                        name: "fk_roleclaims_aspnetroles_identityroleid",
                        column: x => x.roleid,
                        principalSchema: "public",
                        principalTable: "aspnetroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserclaims",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(nullable: false),
                    claimtype = table.Column<string>(nullable: true),
                    claimvalue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userclaims", x => x.id);
                    table.ForeignKey(
                        name: "fk_userclaims_aspnetusers_applicationuserid",
                        column: x => x.userid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserlogins",
                schema: "public",
                columns: table => new
                {
                    loginprovider = table.Column<string>(nullable: false),
                    providerkey = table.Column<string>(nullable: false),
                    providerdisplayname = table.Column<string>(nullable: true),
                    userid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userlogins", x => new { x.loginprovider, x.providerkey });
                    table.ForeignKey(
                        name: "fk_userlogins_aspnetusers_applicationuserid",
                        column: x => x.userid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserroles",
                schema: "public",
                columns: table => new
                {
                    userid = table.Column<string>(nullable: false),
                    roleid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userroles", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "fk_userroles_aspnetroles_identityroleid",
                        column: x => x.roleid,
                        principalSchema: "public",
                        principalTable: "aspnetroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_userroles_aspnetusers_applicationuserid",
                        column: x => x.userid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetusertokens",
                schema: "public",
                columns: table => new
                {
                    userid = table.Column<string>(nullable: false),
                    loginprovider = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usertokens", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "fk_usertokens_aspnetusers_applicationuserid",
                        column: x => x.userid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "earning",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createddate = table.Column<DateTime>(nullable: false),
                    updateddate = table.Column<DateTime>(nullable: false),
                    earningstype = table.Column<string>(nullable: false),
                    amount = table.Column<decimal>(nullable: false),
                    senderid = table.Column<string>(nullable: true),
                    applicationuserid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_earning", x => x.id);
                    table.ForeignKey(
                        name: "fk_earning_users_applicationuserid",
                        column: x => x.applicationuserid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createddate = table.Column<DateTime>(nullable: false),
                    updateddate = table.Column<DateTime>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    views = table.Column<int>(nullable: false),
                    moneyreceived = table.Column<decimal>(nullable: false),
                    moneyspent = table.Column<decimal>(nullable: false),
                    solveranswerid = table.Column<int>(nullable: false),
                    categoryid = table.Column<int>(nullable: false),
                    applicationuserid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_users_applicationuserid",
                        column: x => x.applicationuserid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_question_category_categoryid",
                        column: x => x.categoryid,
                        principalSchema: "public",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answer",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createddate = table.Column<DateTime>(nullable: false),
                    updateddate = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    moneyreceived = table.Column<decimal>(nullable: false),
                    moneyspent = table.Column<decimal>(nullable: false),
                    questionid = table.Column<int>(nullable: false),
                    applicationuserid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answer", x => x.id);
                    table.ForeignKey(
                        name: "fk_answer_users_applicationuserid",
                        column: x => x.applicationuserid,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_answer_question_questionid",
                        column: x => x.questionid,
                        principalSchema: "public",
                        principalTable: "question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questiontag",
                schema: "public",
                columns: table => new
                {
                    questionid = table.Column<int>(nullable: false),
                    tagid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_questiontag", x => new { x.questionid, x.tagid });
                    table.ForeignKey(
                        name: "fk_questiontag_question_questionid",
                        column: x => x.questionid,
                        principalSchema: "public",
                        principalTable: "question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_questiontag_tag_tagid",
                        column: x => x.tagid,
                        principalSchema: "public",
                        principalTable: "tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_answer_applicationuserid",
                schema: "public",
                table: "answer",
                column: "applicationuserid");

            migrationBuilder.CreateIndex(
                name: "ix_answer_questionid",
                schema: "public",
                table: "answer",
                column: "questionid");

            migrationBuilder.CreateIndex(
                name: "ix_roleclaims_roleid",
                schema: "public",
                table: "aspnetroleclaims",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "rolenameindex",
                schema: "public",
                table: "aspnetroles",
                column: "normalizedname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userclaims_userid",
                schema: "public",
                table: "aspnetuserclaims",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userlogins_userid",
                schema: "public",
                table: "aspnetuserlogins",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userroles_roleid",
                schema: "public",
                table: "aspnetuserroles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "emailindex",
                schema: "public",
                table: "aspnetusers",
                column: "normalizedemail");

            migrationBuilder.CreateIndex(
                name: "usernameindex",
                schema: "public",
                table: "aspnetusers",
                column: "normalizedusername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_earning_applicationuserid",
                schema: "public",
                table: "earning",
                column: "applicationuserid");

            migrationBuilder.CreateIndex(
                name: "ix_question_applicationuserid",
                schema: "public",
                table: "question",
                column: "applicationuserid");

            migrationBuilder.CreateIndex(
                name: "ix_question_categoryid",
                schema: "public",
                table: "question",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "ix_questiontag_tagid",
                schema: "public",
                table: "questiontag",
                column: "tagid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetroleclaims",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetuserclaims",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetuserlogins",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetuserroles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetusertokens",
                schema: "public");

            migrationBuilder.DropTable(
                name: "earning",
                schema: "public");

            migrationBuilder.DropTable(
                name: "questiontag",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetroles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "question",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tag",
                schema: "public");

            migrationBuilder.DropTable(
                name: "aspnetusers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "category",
                schema: "public");
        }
    }
}
