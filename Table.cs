using FreeSql.DataAnnotations;

namespace WebApplication3;

[Table(Name = "test_table", DisableSyncStructure = true)]
public class Table
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public ulong Id { get; set; }

    [Column(Name = "password", IsNullable = false)]
    [Aes("somekeyname")]
    public string Password { get; set; }
}