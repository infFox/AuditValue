using FreeSql;

namespace WebApplication3;

public class Repo : BaseRepository<Table, ulong>
{
    public Repo(IFreeSql fsql) : base(fsql, null)
    {
    }
}